using TaskSync.Repositories.Entities;
using TaskSync.Repositories.Interfaces;

namespace TaskSync.Repositories
{
    public class CommentRepository : ICommentRepository
    {
        private readonly AppDbContext _dbContext;
        public CommentRepository(AppDbContext dbContext) => _dbContext = dbContext;

        public async Task InsertAsync(TaskCommentEntity entity)
        {
            await _dbContext.TaskComments.AddAsync(entity);
            await _dbContext.SaveChangesAsync();
        }
    }
}
