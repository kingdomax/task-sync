using Microsoft.EntityFrameworkCore;
using TaskSync.Repositories.Entities;
using TaskSync.Repositories.Interfaces;

namespace TaskSync.Repositories
{
    public class TaskRepository : IRepository<IList<TaskEntity>> // todo: need to have separate interface
    {
        private readonly AppDbContext _dbContext;
        public TaskRepository(AppDbContext dbContext) => _dbContext = dbContext;

        public async Task<IList<TaskEntity>?> GetAsync(int projectId)
        {
            return await _dbContext.Tasks.Where(x => x.ProjectId == projectId).ToListAsync();
        }

        public Task<IList<TaskEntity>?> GetAsync()
        {
            throw new NotImplementedException();
        }

        public Task<IList<TaskEntity>?> GetAsync(string param1)
        {
            throw new NotImplementedException();
        }
    }
}
