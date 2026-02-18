using Microsoft.Extensions.Options;

using RabbitMQ.Client;

using TaskSync.Infrastructure.Settings;

namespace TaskSync.Infrastructure.Messaging
{
    public sealed class RabbitMqTopologyInitializer : IHostedService
    {
        // Topology names (single source of truth)
        public static string MainExchange = "points.award.x";
        public static string MainQueue = "points.award.q";
        public static string MainRoutingKey = "points.award";
        public static string DeadLetterExchange = "points.award.dlx";
        public static string DeadLetterQueue = "points.award.dlq";
        public static string DeadLetterRoutingKey = "points.award.dlq";

        private readonly RabbitMqSettings _settings;

        public RabbitMqTopologyInitializer(IOptions<RabbitMqSettings> options)
        {
            _settings = options.Value;
        }

        public async Task StartAsync(CancellationToken cancellationToken)
        {
            var factory = new ConnectionFactory
            {
                HostName = _settings.Host,
                Port = _settings.Port,
                UserName = _settings.Username,
                Password = _settings.Password,
                VirtualHost = _settings.VirtualHost,
            };

            await using var connection = await factory.CreateConnectionAsync(cancellationToken);
            await using var channel = await connection.CreateChannelAsync();

            // 1) Exchanges
            await channel.ExchangeDeclareAsync(MainExchange, ExchangeType.Direct, durable: true, autoDelete: false, cancellationToken: cancellationToken);
            await channel.ExchangeDeclareAsync(DeadLetterExchange, ExchangeType.Direct, durable: true, autoDelete: false, cancellationToken: cancellationToken);

            // 2) DLQ + bind
            await channel.QueueDeclareAsync(DeadLetterQueue, durable: true, exclusive: false, autoDelete: false, arguments: null, cancellationToken: cancellationToken);
            await channel.QueueBindAsync(DeadLetterQueue, DeadLetterExchange, DeadLetterRoutingKey, cancellationToken: cancellationToken);

            // 3) Main queue with DLQ args + bind
            var args = new Dictionary<string, object>
            {
                ["x-dead-letter-exchange"] = DeadLetterExchange,
                ["x-dead-letter-routing-key"] = DeadLetterRoutingKey,
            };

            await channel.QueueDeclareAsync(MainQueue, durable: true, exclusive: false, autoDelete: false, arguments: args, cancellationToken: cancellationToken);
            await channel.QueueBindAsync(MainQueue, MainExchange, MainRoutingKey, cancellationToken: cancellationToken);
        }

        public Task StopAsync(CancellationToken cancellationToken)
        {
            return Task.CompletedTask;
        }
    }
}
