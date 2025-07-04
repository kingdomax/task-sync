using TaskSync.Repositories.Entities;

namespace TaskSync.Services.Interfaces
{
    public interface IJwtService
    {
        public string GenerateJwtToken(UserEntity user);
    }
}
