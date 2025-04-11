using TaskSync.Models;

namespace TaskSync.Services.Interfaces
{
    public interface IJwtService
    {
        public string GenerateJwtToken(User user);
    }
}
