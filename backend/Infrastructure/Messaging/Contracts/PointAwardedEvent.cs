using TaskSync.Models.Enums;

namespace TaskSync.Infrastructure.Messaging.Contracts
{
    public record PointAwardedEvent(
        Guid EventId, // EventId is important for idempotency (dedupe on consumer side)
        int? UserId,
        int TaskId,
        TASK_STATUS TaskStatus,
        DateTimeOffset OccurredAtUtc,
        string Source
    );
}
