using TaskSync.Models.Dto;
using TaskSync.Services.Interfaces;
using TaskSync.Repositories.Interfaces;
using Microsoft.AspNetCore.SignalR;
using TaskSync.SignalR;
using TaskSync.Infrastructure.Http.Interface;

namespace TaskSync.Services
{
    public class TaskService : ITaskService
    {
        private readonly ITaskRepository _taskRepository;
        private readonly IHubContext<TaskHub> _taskHub;
        private readonly IHttpContextReader _httpContextReader;

        public TaskService(ITaskRepository taskRepository, IHubContext<TaskHub> taskHub, IHttpContextReader httpContextReader)
        {
            _taskRepository = taskRepository;
            _taskHub = taskHub;
            _httpContextReader = httpContextReader;
        }

        public async Task<IList<TaskDto>?> GetTasksAsync(int projectId)
        {
            var taskEntities = await _taskRepository.GetAsync(projectId);

            var result = taskEntities?.Select(x => new TaskDto()
            {
                Id = x.Id,
                Title = x.Title,
                AssigneeId = x.AssigneeId,
                Status = x.Status,
                LastModified = x.LastModified
            }).ToList();

            return result;
        }

        public async Task<TaskDto?> UpdateTaskStatusAsync(int taskId, UpdateTaskRequest request)
        {
            var updatedTask = await _taskRepository.UpdateStatusAsync(taskId, request.StatusRaw);

            if (updatedTask != null)
            {
                var dto = new TaskDto
                {
                    Id = updatedTask.Id,
                    Title = updatedTask.Title,
                    AssigneeId = updatedTask.AssigneeId,
                    Status = updatedTask.Status,
                    LastModified = updatedTask.LastModified
                };

                await PushTaskDto(_httpContextReader.GetConnectionId(), dto);

                return dto;
            }

            return null;
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
