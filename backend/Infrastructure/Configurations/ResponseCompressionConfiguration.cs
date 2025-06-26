using System.IO.Compression;

using Microsoft.AspNetCore.ResponseCompression;

namespace TaskSync.Infrastructure.Configurations
{
    public static class ResponseCompressionConfiguration
    {
        public static IServiceCollection ConfigureResponseCompression(this IServiceCollection services)
        {
            services.AddResponseCompression(options =>
            {
                options.Providers.Add<GzipCompressionProvider>();
                options.MimeTypes = ResponseCompressionDefaults.MimeTypes.Concat(new[] { "application/json" }); // explicitly add JSON
            });

            services.Configure<GzipCompressionProviderOptions>(options =>
            {
                options.Level = CompressionLevel.Fastest;
            });

            return services;
        }
    }
}
