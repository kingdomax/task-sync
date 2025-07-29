using Microsoft.EntityFrameworkCore;

using TaskSync.Repositories.Entities;
using TaskSync.Repositories.Interfaces;

namespace TaskSync.Repositories
{
    public class ProjectRepository : IProjectRepository
    {
        private readonly AppDbContext _dbContext;
        public ProjectRepository(AppDbContext dbContext) => _dbContext = dbContext;

        public async Task<ProjectEntity?> GetByIdAsync(int projectId)
        {
            return await _dbContext.Projects.AsNoTracking().FirstOrDefaultAsync(x => x.Id == projectId);
        }
    }
}
