using TaskSync.Infrastructure.Caching.Interfaces;
using TaskSync.Infrastructure.Http.Interface;
using TaskSync.Models.Dto;
using TaskSync.Repositories.Entities;
using TaskSync.Repositories.Interfaces;
using TaskSync.Services.Interfaces;
using TaskSync.SignalR.Interfaces;

namespace TaskSync.Services
{
    public class TaskService : ITaskService
    {
        private readonly IHttpContextReader _httpContextReader;
        private readonly ITaskRepository _taskRepository;
        private readonly IMemoryCacheService<IList<TaskEntity>> _taskEntityCache;
        private readonly ITaskNotificationService _taskNotificationService;
        private readonly ICacheBackgroundRefresher _cacheBackgroundRefresher;

        public TaskService(
            IHttpContextReader httpContextReader,
            ITaskRepository taskRepository,
            IMemoryCacheService<IList<TaskEntity>> taskEntityCache,
            ITaskNotificationService taskNotificationService,
            ICacheBackgroundRefresher cacheBackgroundRefresher)
        {
            _httpContextReader = httpContextReader;
            _taskRepository = taskRepository;
            _taskEntityCache = taskEntityCache;
            _taskNotificationService = taskNotificationService;
            _cacheBackgroundRefresher = cacheBackgroundRefresher;
        }

        public async Task<IList<TaskDto>?> GetTasksAsync(int projectId)
        {
            var taskEntities = await _taskEntityCache.GetAsync(projectId, async () =>
            {
                return await _taskRepository.GetAsync(projectId);
            });

            return taskEntities?.Select(x => new TaskDto
            {
                Id = x.Id,
                Title = x.Title,
                AssigneeId = x.AssigneeId,
                Status = x.Status,
                LastModified = x.LastModified,
            }).ToList();
        }

        public async Task<TaskDto?> UpdateTaskStatusAsync(int taskId, UpdateTaskRequest request)
        {
            var updatedTask = await _taskRepository.UpdateStatusAsync(taskId, request.StatusRaw);
            if (updatedTask == null)
            {
                return null;
            }

            _taskEntityCache.Remove(updatedTask.ProjectId);
            _cacheBackgroundRefresher.RefreshProjectTasks(updatedTask.ProjectId); // pre-warm cache in other thread pool (i.e. background task)

            var dto = new TaskDto
            {
                Id = updatedTask.Id,
                Title = updatedTask.Title,
                AssigneeId = updatedTask.AssigneeId,
                Status = updatedTask.Status,
                LastModified = updatedTask.LastModified,
            };
            _ = _taskNotificationService.NotifyTaskUpdateAsync(dto, _httpContextReader.GetConnectionId()); // fire and forget, executes on the thread handling the request (non-thread-pooled).
            return dto;
        }

        public async Task<TaskDto> AddTaskAsync(AddTaskRequest request)
        {
            var newTask = await _taskRepository.AddAsync(request.Title, request.AssigneeId, request.ProjectId);

            _taskEntityCache.Remove(newTask.ProjectId);
            _cacheBackgroundRefresher.RefreshProjectTasks(newTask.ProjectId);

            var dto = new TaskDto
            {
                Id = newTask.Id,
                Title = newTask.Title,
                AssigneeId = newTask.AssigneeId,
                Status = newTask.Status,
                LastModified = newTask.LastModified,
            };
            _ = _taskNotificationService.NotifyTaskUpdateAsync(dto, _httpContextReader.GetConnectionId());
            return dto;
        }
    }
}
