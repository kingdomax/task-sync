using Microsoft.Extensions.Options;

using TaskSync.Infrastructure.Settings;

namespace TaskSync.Infrastructure.Configurations
{
    public static class CorsConfiguration
    {
        public static IServiceCollection ConfigureCors(this IServiceCollection services)
        {
            var provider = services.BuildServiceProvider();

            services.AddCors(options =>
            {
                options.AddDefaultPolicy(policy =>
                {
                    policy.WithOrigins(provider.GetRequiredService<IOptions<FrontendSettings>>().Value.Urls)
                        .AllowAnyHeader()
                        .AllowAnyMethod()
                        .AllowCredentials()
                        .SetPreflightMaxAge(TimeSpan.FromMinutes(10)); // Cache preflight 10 min
                });
            });

            return services;
        }
    }
}
