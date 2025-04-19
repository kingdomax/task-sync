using TaskSync.Models.Dto;

namespace TaskSync.Services.Interfaces
{
    public interface ITaskService
    {
        public Task<IList<TaskDto>?> GetTasksAsync(int projectId);
    }
}