using MassTransit;

using Microsoft.Extensions.Options;

using TaskSync.Infrastructure.Settings;

namespace TaskSync.Infrastructure.Configurations
{
    public static class MessagingConfiguration
    {
        public static IServiceCollection AddMessaging(this IServiceCollection services)
        {
            var provider = services.BuildServiceProvider();

            services.AddMassTransit(x =>
            {
                x.UsingRabbitMq((context, cfg) =>
                {
                    var settings = provider.GetRequiredService<IOptions<RabbitMqSettings>>().Value;

                    cfg.Host(settings.Host, "/", h =>
                    {
                        h.Username(settings.Username);
                        h.Password(settings.Password);
                    });

                    cfg.ConfigureEndpoints(context);
                });
            });

            return services;
        }
    }
}
