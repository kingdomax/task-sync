namespace TaskSync.Infrastructure.Http.Interface
{
    public interface IHttpContextReader
    {
        string? GetConnectionId();
        string? GetUsername();
        string? GetUserId();
        string? GetCookie(string cookieKey);
    }
}
