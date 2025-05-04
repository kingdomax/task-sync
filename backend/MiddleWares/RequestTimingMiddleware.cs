using System.Diagnostics;
using Microsoft.Extensions.Options;
using TaskSync.Infrastructure.Settings;

namespace TaskSync.MiddleWares
{
    public class RequestTimingMiddleware
    {
        private readonly RequestDelegate _next;
        private readonly List<PathString> _excludedPaths;

        public RequestTimingMiddleware(RequestDelegate next, IOptions<MiddlewareSettings> options)
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

            var sw = Stopwatch.StartNew();

            await _next(context);

            sw.Stop();
            Console.WriteLine($"[TimingMiddleware] {sw.ElapsedMilliseconds}ms");
        }
    }
}
