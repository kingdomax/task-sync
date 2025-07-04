using TaskSync.Repositories.Entities;
using TaskSync.Repositories.Interfaces;
using TaskSync.Services.Interfaces;

namespace TaskSync.Services
{
    public class CommentService : ICommentService
    {
        private readonly ICommentRepository _commentRepository;

        public CommentService(ICommentRepository commentRepository)
        {
            _commentRepository = commentRepository;
        }

        public async Task AddTaskCreatedCommentAsync(TaskEntity task, ProjectEntity project, string username)
        {
            var comment = new TaskCommentEntity
            {
                TaskId = task.Id,
                CommenterId = task.CreatorId!.Value,
                CreatedAt = DateTime.UtcNow,
                TypeRaw = "task_created",
                CommentText = $"{username} added this task to project {project.Name}",
                MetaData = new Dictionary<string, object>
                {
                    { "event", "task_created" },
                    { "task_id", task.Id },
                    { "username", username },
                    { "project_id", project.Id },
                    { "project_name",  project.Name },
                },
            };

            await _commentRepository.InsertAsync(comment);
        }
    }
}
