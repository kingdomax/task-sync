using TaskSync.Repositories.Entity;
using Microsoft.EntityFrameworkCore;

namespace TaskSync.Repositories
{
    public class UserRepository : IRepository<UserEntity>
    {
        private readonly AppDbContext _dbContext;

        public UserRepository(AppDbContext dbContext) => _dbContext = dbContext;

        public async Task<UserEntity?> GetAsync()
        {
            return await _dbContext.Users.SingleOrDefaultAsync(x => x.Id == 1);
        }
    }

    // Segregate interface pattern (SOLID)
    public interface IRepository<T>
    {
        public Task<T?> GetAsync();
    }
}