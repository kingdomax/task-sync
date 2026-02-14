using TaskSync.Models.Enums;

namespace TaskSync.Infrastructure.Messaging.Interfaces
{
    public interface IPointsEventPublisher
    {
        Task PublishPointAwardedAsync(int taskId, TASK_STATUS status, CancellationToken ct = default);
    }
}
