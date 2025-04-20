using TaskSync.Repositories.Entities;

namespace TaskSync.Repositories.Interfaces
{
    public interface ITaskRepository
    {
        public Task<IList<TaskEntity>?> GetAsync(int projectId);
        public Task<TaskEntity?> UpdateStatusAsync(int taskId, string newStatus);
    }
}
