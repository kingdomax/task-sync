using TaskSync.Services.Interfaces;
using TaskSync.Repositories.Interfaces;
using TaskSync.Models.Dto;
using TaskSync.Repositories.Entities;
using TaskSync.Repositories;

namespace TaskSync.Services
{
    public class TaskService : ITaskService
    {
        private readonly ITaskRepository _taskRepository;

        public TaskService(ITaskRepository taskRepository) 
        {
            _taskRepository = taskRepository;
        }

        public async Task<IList<TaskDto>?> GetTasksAsync(int projectId)
        {
            var taskEntities = await _taskRepository.GetAsync(projectId);

            var result = taskEntities?.Select(x => new TaskDto()
            {
                Id = x.Id,
                Title = x.Title,
                AssigneeId = x.AssigneeId,
                Status = x.Status,
                LastModified = x.LastModified
            }).ToList();

            return result;
        }

        public async Task<TaskDto?> UpdateTaskStatusAsync(int taskId, UpdateTaskRequest request)
        {
            var updatedTask = await _taskRepository.UpdateStatusAsync(taskId, request.StatusRaw);

            return updatedTask != null ? new TaskDto() 
            {
                Id = updatedTask.Id,
                Title = updatedTask.Title,
                AssigneeId = updatedTask.AssigneeId,
                Status = updatedTask.Status,
                LastModified = updatedTask.LastModified
            } : null;
        }
    }
}
