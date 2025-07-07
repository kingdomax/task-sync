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
        private readonly ICommentService _commentService;
        private readonly IProjectRepository _projectRepository;

        // todo-moch: dependencies start to grow, maybe consider using MediatR or similar patterns
        public TaskService(
            IHttpContextReader httpContextReader,
            ITaskRepository taskRepository,
            IMemoryCacheService<IList<TaskEntity>> taskEntityCache,
            ITaskNotificationService taskNotificationService,
            ICacheBackgroundRefresher cacheBackgroundRefresher,
            IGamificationApi gamificationApi,
            ICommentService commentService,
            IProjectRepository projectRepository)
        {
            _httpContextReader = httpContextReader;
            _taskRepository = taskRepository;
            _taskEntityCache = taskEntityCache;
            _taskNotificationService = taskNotificationService;
            _cacheBackgroundRefresher = cacheBackgroundRefresher;
            _gamificationApi = gamificationApi;
            _commentService = commentService;
            _projectRepository = projectRepository;
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
            // Add task record to "tasks" table and comment record to "task_comments" table
            TaskEntity newTask;
            var project = await _projectRepository.GetByIdAsync(projectId);
            using (var tx = await _taskRepository.BeginTransactionAsync())
            {
                newTask = await _taskRepository.AddAsync(request.Title, request.AssigneeId, projectId, _httpContextReader.GetUserId());
                await _commentService.AddTaskCreatedCommentAsync(newTask, project!, _httpContextReader.GetUsername()!);
                await tx.CommitAsync();
            }

            // Mapping TaskEntity to TaskDto
            var newTaskDto = new TaskDto
            {
                Id = newTask.Id,
                Title = newTask.Title,
                AssigneeId = newTask.AssigneeId,
                Status = newTask.Status,
                LastModified = newTask.LastModified,
            };

            // Perform side effect without waiting for completion
            _cacheBackgroundRefresher.RefreshProjectTasks(projectId);
            _ = _gamificationApi.UpdatePoint(newTaskDto.Id, TASK_STATUS.CREATE);
            _ = _taskNotificationService.NotifyTaskCreateAsync(newTaskDto, _httpContextReader.GetConnectionId());

            return newTaskDto;
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
