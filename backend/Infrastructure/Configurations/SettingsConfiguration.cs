﻿using TaskSync.Infrastructure.Settings;

namespace TaskSync.Infrastructure.Configurations
{
    public static class SettingsConfiguration
    {
        public static IServiceCollection ConfigureAppSettings(this IServiceCollection services, IConfiguration configuration)
        {
            services.Configure<JwtSettings>(configuration.GetSection("JwtSettings"));
            services.Configure<MiddlewareSettings>(configuration.GetSection("MiddlewareSettings"));

            return services;
        }
    }
}
