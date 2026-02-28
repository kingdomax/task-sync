using System.Text;
using System.Text.Json;

using Microsoft.Extensions.Options;

using RabbitMQ.Client;

using TaskSync.Infrastructure.Messaging.Contracts;
using TaskSync.Infrastructure.Messaging.Interfaces;
using TaskSync.Infrastructure.Settings;
using TaskSync.Models.Enums;

namespace TaskSync.Infrastructure.Messaging
{
    // sealed for better performance, since it won't be inherited by other classes
    // used with service, repo, api, infra, any class that need no inheritance
    public sealed class PointsEventPublisher : IPointsEventPublisher, IAsyncDisposable
    {
        private readonly ConnectionFactory _factory;
        private IConnection? _connection;

        public PointsEventPublisher(IOptions<RabbitMqSettings> options)
        {
            var s = options.Value;
            _factory = new ConnectionFactory
            {
                HostName = s.Host,
                Port = s.Port,
                UserName = s.Username,
                Password = s.Password,
                VirtualHost = s.VirtualHost,
            };
        }

        // RabbitMQ Overview: https://www.youtube.com/watch?v=deG25y_r6OY
        // Producer(.NET) -> Exchange(points.award.x) -> Queue(points.award.q) -> Consumer(NestJs)
        public async Task PublishPointAwardedAsync(int taskId, TASK_STATUS status, int? userId, CancellationToken ct = default)
        {
            var connection = await GetConnectionAsync(ct);
            await using var channel = await connection.CreateChannelAsync();

            var evt = new PointAwardedEvent(
                EventId: Guid.NewGuid(),
                UserId: userId,
                TaskId: taskId,
                TaskStatus: status,
                OccurredAtUtc: DateTimeOffset.UtcNow,
                Source: "tasksync-backend"
            );
            var body = Encoding.UTF8.GetBytes(JsonSerializer.Serialize(evt));
            var props = new BasicProperties
            {
                Persistent = true,
                ContentType = "application/json",
            };

            await channel.BasicPublishAsync(
                exchange: RabbitMqTopologyInitializer.MainExchange,
                routingKey: RabbitMqTopologyInitializer.MainRoutingKey,
                mandatory: false,
                basicProperties: props,
                body: body,
                cancellationToken: ct
            );
            Console.WriteLine($"[PointsEventPublisher] Published -> {RabbitMqTopologyInitializer.MainExchange} rk={RabbitMqTopologyInitializer.MainRoutingKey}: userId={evt.UserId}, taskId={evt.TaskId}, status={evt.TaskStatus}");
        }

        public async ValueTask DisposeAsync()
        {
            if (_connection != null)
            {
                await _connection.DisposeAsync();
            }
        }

        private async Task<IConnection> GetConnectionAsync(CancellationToken ct)
        {
            if (_connection != null)
            {
                return _connection;
            }
            _connection = await _factory.CreateConnectionAsync(ct);
            return _connection;
        }
    }
}
