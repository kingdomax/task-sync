using TaskSync.Repositories.Entities;

namespace TaskSync.Repositories.Interfaces
{
    public interface ITaskRepository
    {
        Task<IList<TaskEntity>?> GetAsync(int projectId);
        Task<TaskEntity?> UpdateStatusAsync(int taskId, string newStatus);
        Task<TaskEntity> AddAsync(string title, int? assigneeId, int projectId, int? creatorId);
        Task<TaskEntity?> DeleteAsync(int taskId);
    }
}
