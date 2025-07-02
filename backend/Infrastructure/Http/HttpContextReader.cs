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

        public string? GetUsername()
        {
            return _httpContextAccessor.HttpContext?.User.FindFirstValue(ClaimTypes.Name);
        }

        public string? GetUserId()
        {
            return _httpContextAccessor.HttpContext?.User.FindFirstValue(ClaimTypes.NameIdentifier);
        }

        public string? GetCookie(string cookieKey)
        {
            return _httpContextAccessor.HttpContext?.Request.Cookies[cookieKey]; // read
            // Response.Cookies.Append("MyCookieKey", "my_value", new CookieOptions // write
            // {
            //     HttpOnly = true, // javascript cannot access
            //     Secure = true,   // only https
            //     SameSite = SameSiteMode.Strict,
            //     Expires = DateTimeOffset.UtcNow.AddDays(7)
            // });
            //
            // Response.Cookies.Delete("MyCookieKey"); // delete
        }
    }
}
