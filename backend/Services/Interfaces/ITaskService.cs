using TaskSync.Models.Dto;

namespace TaskSync.Services.Interfaces
{
    public interface ITaskService
    {
        Task<IList<TaskDto>?> GetTasksAsync(int projectId);
        Task<TaskDto?> UpdateTaskStatusAsync(int taskId, UpdateTaskRequest request);
        Task<TaskDto> AddTaskAsync(AddTaskRequest request);
        Task<bool> DeleteTaskAsync(int taskId);
    }
}
