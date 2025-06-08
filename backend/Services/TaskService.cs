using TaskSync.Enums;
using TaskSync.ExternalApi.Interfaces;
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
        private readonly IGamificationApi _gamificationApi;

        public TaskService(
            IHttpContextReader httpContextReader,
            ITaskRepository taskRepository,
            IMemoryCacheService<IList<TaskEntity>> taskEntityCache,
            ITaskNotificationService taskNotificationService,
            ICacheBackgroundRefresher cacheBackgroundRefresher,
            IGamificationApi gamificationApi)
        {
            _httpContextReader = httpContextReader;
            _taskRepository = taskRepository;
            _taskEntityCache = taskEntityCache;
            _taskNotificationService = taskNotificationService;
            _cacheBackgroundRefresher = cacheBackgroundRefresher;
            _gamificationApi = gamificationApi;
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

        public async Task<TaskDto> AddTaskAsync(int projectId, AddTaskRequest request)
        {
            TaskEntity newTask = await _taskRepository.AddAsync(request.Title, request.AssigneeId, projectId);

            TaskDto dto = new TaskDto
            {
                Id = newTask.Id,
                Title = newTask.Title,
                AssigneeId = newTask.AssigneeId,
                Status = newTask.Status,
                LastModified = newTask.LastModified,
            };

            _cacheBackgroundRefresher.RefreshProjectTasks(projectId); // pre-warm cache in other thread pool (i.e. background task)
            _ = _gamificationApi.UpdatePoint(dto.Id, TASK_STATUS.CREATE); // fire and forget, executes on the thread handling the request (non-thread-pooled).
            _ = _taskNotificationService.NotifyTaskCreateAsync(dto, _httpContextReader.GetConnectionId());
            return dto;
        }

        public async Task<TaskDto?> UpdateTaskStatusAsync(int taskId, UpdateTaskRequest request)
        {
            TaskEntity? updatedTask = await _taskRepository.UpdateStatusAsync(taskId, request.StatusRaw);
            if (updatedTask == null)
            {
                return null;
            }

            TaskDto dto = new TaskDto
            {
                Id = updatedTask.Id,
                Title = updatedTask.Title,
                AssigneeId = updatedTask.AssigneeId,
                Status = updatedTask.Status,
                LastModified = updatedTask.LastModified,
            };

            _cacheBackgroundRefresher.RefreshProjectTasks(updatedTask.ProjectId);
            _ = _gamificationApi.UpdatePoint(dto.Id, dto.Status);
            _ = _taskNotificationService.NotifyTaskUpdateAsync(dto, _httpContextReader.GetConnectionId());
            return dto;
        }

        public async Task<bool> DeleteTaskAsync(int taskId)
        {
            TaskEntity? deletedTask = await _taskRepository.DeleteAsync(taskId);
            if (deletedTask == null)
            {
                return false;
            }

            _cacheBackgroundRefresher.RefreshProjectTasks(deletedTask.ProjectId);
            _ = _gamificationApi.UpdatePoint(taskId, TASK_STATUS.DELETE);
            _ = _taskNotificationService.NotifyTaskDeleteAsync(taskId, _httpContextReader.GetConnectionId());
            return true;
        }
    }
}
