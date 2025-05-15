using TaskSync.Models.Dto;

namespace TaskSync.SignalR.Interfaces
{
    public interface ITaskNotificationService
    {
        Task NotifyTaskCreateAsync(TaskDto dto, string? excludeConnectionId);
        Task NotifyTaskUpdateAsync(TaskDto dto, string? excludeConnectionId);
        Task NotifyTaskDeleteAsync(int taskId, string? excludeConnectionId);
    }
}
