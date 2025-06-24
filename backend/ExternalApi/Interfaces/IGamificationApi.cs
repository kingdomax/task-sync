using TaskStatus = TaskSync.Enums.TASK_STATUS;

namespace TaskSync.ExternalApi.Interfaces
{
    public interface IGamificationApi
    {
        Task UpdatePoint(int taskId, TaskStatus status);
    }
}
