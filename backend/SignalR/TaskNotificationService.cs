using Microsoft.AspNetCore.SignalR;

using TaskSync.Enums;
using TaskSync.Models.Dto;
using TaskSync.SignalR.Interfaces;

namespace TaskSync.SignalR
{
    public class TaskNotificationService : ITaskNotificationService
    {
        private readonly IHubContext<TaskHub> _taskHubContext;

        public TaskNotificationService(IHubContext<TaskHub> taskHubContext) => _taskHubContext = taskHubContext;

        public async Task NotifyTaskUpdateAsync(TaskDto dto, string? excludeConnectionId) => await NotifyAsync(new NotifyTask(NOTIFY_STATUS.UPDATE, dto), excludeConnectionId);
        public async Task NotifyTaskCreateAsync(TaskDto dto, string? excludeConnectionId) => await NotifyAsync(new NotifyTask(NOTIFY_STATUS.CREATE, dto), excludeConnectionId);
        public async Task NotifyTaskDeleteAsync(int taskId, string? excludeConnectionId) => await NotifyAsync(new NotifyTask(NOTIFY_STATUS.DELETE, new TaskDto() { Id = taskId, Title = string.Empty }), excludeConnectionId);

        private async Task NotifyAsync(NotifyTask data, string? excludeConnectionId)
        {
            if (!string.IsNullOrWhiteSpace(excludeConnectionId))
            {
                await _taskHubContext.Clients.AllExcept(excludeConnectionId).SendAsync("TaskUpdated", data);
            }
            else
            {
                await _taskHubContext.Clients.All.SendAsync("TaskUpdated", data);
            }
        }
    }
}
