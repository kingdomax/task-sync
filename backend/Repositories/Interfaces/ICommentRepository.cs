using TaskSync.Repositories.Entities;

namespace TaskSync.Repositories.Interfaces
{
    public interface ICommentRepository
    {
        Task InsertAsync(TaskCommentEntity entity);
    }
}
