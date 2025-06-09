using System.Text.Json;

namespace TaskSync.MiddleWares
{
    public class ExceptionHandlingMiddleware
    {
        private readonly RequestDelegate _next;
        private readonly ILogger<ExceptionHandlingMiddleware> _logger;

        public ExceptionHandlingMiddleware(RequestDelegate next, ILogger<ExceptionHandlingMiddleware> logger)
        {
            _next = next;
            _logger = logger;
        }

        public async Task InvokeAsync(HttpContext context)
        {
            try
            {
                await _next.Invoke(context);
            }
            catch (Exception ex)
            {
                await HandleExceptionResponseAsync(context, ex);
                _logger.LogError(ex, ex.Message);
            }
        }

        private Task HandleExceptionResponseAsync(HttpContext context, Exception exception)
        {
            context.Response.ContentType = "application/json";
            context.Response.StatusCode = StatusCodes.Status500InternalServerError;

            var result = JsonSerializer.Serialize(new
            {
                error = exception.Message,
                stacktrace = exception.StackTrace,
                detail = "An internal server error occurred. Please contact support.",
            });

            return context.Response.WriteAsync(result);
        }
    }
}
