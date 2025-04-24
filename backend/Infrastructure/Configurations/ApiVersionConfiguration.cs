using Microsoft.AspNetCore.Mvc;

namespace TaskSync.Infrastructure.Configurations
{
    public static class ApiVersionConfiguration
    {
        public static IServiceCollection ConfigureApiVersion(this IServiceCollection services)
        {
            services.AddApiVersioning(options =>
            {
                options.AssumeDefaultVersionWhenUnspecified = true;
                options.DefaultApiVersion = new ApiVersion(1, 0);
                options.ReportApiVersions = true; // Adds response headers: api-supported-versions
            });

            return services;
        }
    }
}
