using System.Threading.RateLimiting;

namespace TaskSync.Infrastructure.Configurations
{
    public static class RateLimitConfiguration
    {
        public static IServiceCollection AddRateLimiter(this IServiceCollection services)
        {
            services.AddRateLimiter(options =>
            {
                options.GlobalLimiter = PartitionedRateLimiter.Create<HttpContext, string>(context =>
                {
                    // Key by IP address
                    var ip = context.Connection.RemoteIpAddress?.ToString() ?? "unknown";

                    return RateLimitPartition.GetTokenBucketLimiter(ip, _ => new TokenBucketRateLimiterOptions
                    {
                        TokenLimit = 10, // max requests
                        QueueProcessingOrder = QueueProcessingOrder.OldestFirst,
                        QueueLimit = 0, // reject excess
                        ReplenishmentPeriod = TimeSpan.FromSeconds(10),
                        TokensPerPeriod = 10,
                        AutoReplenishment = true,
                    });
                });

                options.RejectionStatusCode = 429; // Too Many Requests
            });

            return services;
        }
    }
}
