using TaskSync.Models;

namespace TaskSync.Services.Interfaces
{
    public interface IUserService
    {
        public Task<User?> GetUserAsync();
        public Task<User?> GetUserAsync(string email);
    }
}