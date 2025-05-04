using Microsoft.EntityFrameworkCore;

using TaskSync.Repositories.Entities;
using TaskSync.Repositories.Interfaces;

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

        public async Task<UserEntity?> GetAsync(string email)
        {
            return await _dbContext.Users.SingleOrDefaultAsync(x => x.Email == email);
        }

        public Task<UserEntity?> GetAsync(int param1)
        {
            throw new NotImplementedException();
        }
    }
}