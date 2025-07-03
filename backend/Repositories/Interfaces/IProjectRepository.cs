using TaskSync.Repositories.Entities;

namespace TaskSync.Repositories.Interfaces
{
    public interface IProjectRepository
    {
        Task<ProjectEntity?> GetByIdAsync(int projectId);
    }
}
