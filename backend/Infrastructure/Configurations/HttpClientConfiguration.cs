using Microsoft.Extensions.Options;

using TaskSync.Infrastructure.Settings;

namespace TaskSync.Infrastructure.Configurations
{
    public static class HttpClientConfiguration
    {
        public static IServiceCollection ConfigureHttpClient(this IServiceCollection services)
        {
            var provider = services.BuildServiceProvider();

            services.AddHttpClient("GamificationApi", client =>
            {
                client.BaseAddress = new Uri(provider.GetRequiredService<IOptions<GamificationApiSettings>>().Value.BaseUrl);
                client.DefaultRequestHeaders.Add("x-gamapi-auth", "true");
            });

            return services;
        }
    }
}
