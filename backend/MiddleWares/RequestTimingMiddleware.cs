using System.Diagnostics;

namespace TaskSync.MiddleWares
{
    public class RequestTimingMiddleware
    {
        private readonly RequestDelegate _next;

        public RequestTimingMiddleware(RequestDelegate next)
        {
            _next = next;
        }

        public async Task InvokeAsync(HttpContext context)
        {
            var sw = Stopwatch.StartNew();

            await _next(context);

            sw.Stop();
            Console.WriteLine($"[Timing Middleware] {context.Request.Method} {context.Request.Path} - {sw.ElapsedMilliseconds}ms");
        }
    }
}
