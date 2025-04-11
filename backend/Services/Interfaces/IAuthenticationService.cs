using TaskSync.Models.Request;

namespace TaskSync.Services.Interfaces
{
    public interface IAuthenticationService
    {
        public Task<string?> Authenticate(LoginRequest request);
    }
}
