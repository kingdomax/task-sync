namespace TaskSync.Infrastructure.Http.Interface
{
    public interface IHttpContextReader
    {
        string? GetConnectionId();
        string? GetUsername();
        int? GetUserId();
        string? GetCookie(string cookieKey);
    }
}
