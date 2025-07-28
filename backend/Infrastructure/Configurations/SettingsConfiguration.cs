using TaskSync.Infrastructure.Settings;

namespace TaskSync.Infrastructure.Configurations
{
    public static class SettingsConfiguration
    {
        public static IServiceCollection ConfigureAppSettings(this IServiceCollection services, IConfiguration configuration)
        {
            services.Configure<AppInfo>(configuration.GetSection("AppInfo"));
            services.Configure<JwtSettings>(configuration.GetSection("JwtSettings"));
            services.Configure<FrontendSettings>(configuration.GetSection("Frontend"));
            services.Configure<PostgreSqlSettings>(configuration.GetSection("PostgreSql"));
            services.Configure<OtelCollectorSettings>(configuration.GetSection("OtelCollector"));
            services.Configure<MiddlewareSettings>(configuration.GetSection("MiddlewareSettings"));
            services.Configure<GamificationApiSettings>(configuration.GetSection("GamificationApi"));

            return services;
        }
    }
}
