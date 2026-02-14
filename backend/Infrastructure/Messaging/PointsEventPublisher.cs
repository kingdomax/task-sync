using MassTransit;

using TaskSync.Infrastructure.Http.Interface;
using TaskSync.Infrastructure.Messaging.Contracts;
using TaskSync.Infrastructure.Messaging.Interfaces;
using TaskSync.Models.Enums;

namespace TaskSync.Infrastructure.Messaging
{
    public class PointsEventPublisher : IPointsEventPublisher
    {
        private readonly IPublishEndpoint _publish;
        private readonly IHttpContextReader _httpContextReader;

        public PointsEventPublisher(IPublishEndpoint publish, IHttpContextReader httpContextReader)
        {
            _publish = publish;
            _httpContextReader = httpContextReader;
        }

        public async Task PublishPointAwardedAsync(int taskId, TASK_STATUS status, CancellationToken ct = default)
        {
            var evt = new PointAwardedEvent(
                EventId: Guid.NewGuid(),
                UserId: _httpContextReader.GetUserId(),
                TaskId: taskId,
                TaskStatus: status,
                OccurredAtUtc: DateTimeOffset.UtcNow,
                Source: "tasksync-backend"
            );

            await _publish.Publish(evt, ct); // Fanout to interested consumers (MassTransit will route)
        }
    }
}
