namespace TaskSync.Infrastructure.Http.Interface
{
    public interface IHttpContextReader
    {
        string? GetConnectionId();
        string GetUserId();
        string? GetCookie(string cookieKey);
    }
}
