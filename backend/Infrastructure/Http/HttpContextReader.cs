using System.Security.Claims;

using TaskSync.Infrastructure.Http.Interface;

namespace TaskSync.Infrastructure.Http
{
    public class HttpContextReader : IHttpContextReader
    {
        private readonly IHttpContextAccessor _httpContextAccessor;
        public HttpContextReader(IHttpContextAccessor httpContextAccessor) => _httpContextAccessor = httpContextAccessor;

        public string? GetConnectionId()
        {
            return _httpContextAccessor.HttpContext?.Request.Headers["x-connection-id"].FirstOrDefault();
        }

        public string GetUserId()
        {
            return _httpContextAccessor.HttpContext?.User.FindFirstValue(ClaimTypes.NameIdentifier) ?? string.Empty;
        }
    }
}
