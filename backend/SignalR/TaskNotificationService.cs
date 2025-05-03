using Microsoft.AspNetCore.SignalR;
using TaskSync.SignalR.Interfaces;
using TaskSync.Models.Dto;

namespace TaskSync.SignalR
{
    public class TaskNotificationService : ITaskNotificationService
    {
        private readonly IHubContext<TaskHub> _taskHubContext;

        public TaskNotificationService(IHubContext<TaskHub> taskHubContext)
        {
            _taskHubContext = taskHubContext;
        }

        public async Task NotifyTaskUpdateAsync(TaskDto dto, string? excludeConnectionId = null)
        {
            if (!string.IsNullOrWhiteSpace(excludeConnectionId))
            {
                await _taskHubContext.Clients.AllExcept(excludeConnectionId).SendAsync("TaskUpdated", dto);
            }
            else
            { 
                await _taskHubContext.Clients.All.SendAsync("TaskUpdated", dto);
            }
        }
    }
}
