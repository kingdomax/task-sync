using TaskSync.Services.Interfaces;
using TaskSync.Repositories.Interfaces;
using TaskSync.Models.Dto;
using TaskSync.Repositories.Entities;

namespace TaskSync.Services
{
    public class TaskService : ITaskService
    {
        private readonly IRepository<IList<TaskEntity>> _taskRepository;

        public TaskService(IRepository<IList<TaskEntity>> taskRepository) 
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
    }
}
