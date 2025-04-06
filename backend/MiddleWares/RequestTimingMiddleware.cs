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
            Console.WriteLine($"--- [Timing] Started {context.Request.Method} {context.Request.Path} ---");

            await _next(context);

            sw.Stop();
            Console.WriteLine($"--- [Timing] Finished in {sw.ElapsedMilliseconds}ms ---");
        }
    }
}
