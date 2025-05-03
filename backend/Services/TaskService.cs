using TaskSync.Models.Dto;
using TaskSync.Services.Interfaces;
using TaskSync.Repositories.Entities;
using TaskSync.Repositories.Interfaces;
using Microsoft.AspNetCore.SignalR;
using TaskSync.SignalR;
using TaskSync.Infrastructure.Http.Interface;
using TaskSync.Infrastructure.Caching.Interfaces;

namespace TaskSync.Services
{
    public class TaskService : ITaskService
    {
        private readonly ITaskRepository _taskRepository;
        private readonly IHubContext<TaskHub> _taskHub;
        private readonly IHttpContextReader _httpContextReader;
        private readonly IMemoryCacheService<IList<TaskEntity>> _taskEntityCache;

        public TaskService(
            ITaskRepository taskRepository,
            IHubContext<TaskHub> taskHub,
            IHttpContextReader httpContextReader,
            IMemoryCacheService<IList<TaskEntity>> taskEntityCache)
        {
            _taskRepository = taskRepository;
            _taskHub = taskHub;
            _httpContextReader = httpContextReader;
            _taskEntityCache = taskEntityCache;
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
                LastModified = x.LastModified
            }).ToList();
        }

        public async Task<TaskDto?> UpdateTaskStatusAsync(int taskId, UpdateTaskRequest request)
        {
            var updatedTask = await _taskRepository.UpdateStatusAsync(taskId, request.StatusRaw);

            if (updatedTask == null) { return null; }

            var dto = new TaskDto
            {
                Id = updatedTask.Id,
                Title = updatedTask.Title,
                AssigneeId = updatedTask.AssigneeId,
                Status = updatedTask.Status,
                LastModified = updatedTask.LastModified
            };

            _taskEntityCache.Remove(updatedTask.ProjectId);
            await PushTaskDto(_httpContextReader.GetConnectionId(), dto);

            return dto;
        }

        private async Task PushTaskDto(string? connectionIdToExclude, TaskDto taskDto)
        {
            if (!string.IsNullOrWhiteSpace(connectionIdToExclude))
            {
                await _taskHub.Clients.AllExcept(connectionIdToExclude).SendAsync("TaskUpdated", taskDto);
            }
            else
            {
                await _taskHub.Clients.All.SendAsync("TaskUpdated", taskDto);
            }
        }
    }
}
