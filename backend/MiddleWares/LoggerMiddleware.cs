using Microsoft.Extensions.Options;
using TaskSync.Infrastructure.Settings;

namespace TaskSync.MiddleWares
{
    public class LoggerMiddleware
    {
        private readonly RequestDelegate _next;
        private readonly List<PathString> _excludedPaths;

        public LoggerMiddleware(RequestDelegate next, IOptions<MiddlewareSettings> options)
        {
            _next = next;
            _excludedPaths = options.Value.ExcludedPaths.Select(p => new PathString(p)).ToList();
        }

        public async Task InvokeAsync(HttpContext context)
        {
            if (_excludedPaths.Any(p => context.Request.Path.StartsWithSegments(p)))
            {
                await _next(context);
                return;
            }

            Console.WriteLine($"-------------------");
            Console.WriteLine($"[LoggerMiddleware] {context.Request.Method} {context.Request.Path}");
            await _next(context);
            Console.WriteLine($"-------------------");
        }
    }
}
