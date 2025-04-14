using TaskSync.Models.Request;

namespace TaskSync.Services.Interfaces
{
    public interface IAuthenticationService
    {
        public Task<string?> AuthenticateAsync(LoginRequest request);
    }
}
