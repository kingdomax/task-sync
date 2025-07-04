using TaskSync.Repositories.Entities;

namespace TaskSync.Services.Interfaces
{
    public interface ICommentService
    {
        Task AddTaskCreatedCommentAsync(TaskEntity task, ProjectEntity project, string username);
    }
}
