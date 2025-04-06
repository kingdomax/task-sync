namespace TaskSync.MiddleWares
{
    public class RequestLoggerMiddleware
    {
        private readonly RequestDelegate _next;

        public RequestLoggerMiddleware(RequestDelegate next)
        {
            _next = next;
        }

        public async Task InvokeAsync(HttpContext context)
        {
            Console.WriteLine("--- Before RequestLoggerMiddleware ---");
            
            await _next(context);
            
            Console.WriteLine("--- After RequestLoggerMiddleware ---");
        }
    }
}
