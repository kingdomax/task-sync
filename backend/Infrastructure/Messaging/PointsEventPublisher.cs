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
        private readonly ISendEndpointProvider _sendEndpointProvider;

        public PointsEventPublisher(IPublishEndpoint publish, ISendEndpointProvider sendEndpointProvider, IHttpContextReader httpContextReader)
        {
            _publish = publish;
            _sendEndpointProvider = sendEndpointProvider;
            _httpContextReader = httpContextReader;
        }

        // RabbitMQ Overview: https://www.youtube.com/watch?v=deG25y_r6OY
        // Consumer(.NET) -> Exchange(RabbitMQ's points.award.queue) -> Queue(RabbitMQ's points.award.queue) -> Consumer(NestJs)
        public async Task PublishPointAwardedAsync(int taskId, TASK_STATUS status, CancellationToken ct = default)
        {
            Console.WriteLine($"[PointsEventPublisher] Publishing PointAwardedEvent = userId:{_httpContextReader.GetUserId()}, taskid:{taskId}, status:{status}");
            var evt = new PointAwardedEvent(
                EventId: Guid.NewGuid(),
                UserId: _httpContextReader.GetUserId(),
                TaskId: taskId,
                TaskStatus: status,
                OccurredAtUtc: DateTimeOffset.UtcNow,
                Source: "tasksync-backend"
            ); // todo-moch: need to pass EventId into database for idempotency check, otherwise the consumer might  process it twice and award points twice

            var endpoint = await _sendEndpointProvider.GetSendEndpoint(new Uri("queue:points.award.queue"));
            await endpoint.Send(evt, ct); // MassTransit automatically create exchange "points.award.queue" and bind to the queue
        }
    }
}
