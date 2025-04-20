using TaskSync.Models.Dto;

namespace TaskSync.Services.Interfaces
{
    public interface IAuthenticationService
    {
        public Task<string?> AuthenticateAsync(LoginRequest request);
    }
}
