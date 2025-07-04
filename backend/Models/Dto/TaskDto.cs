using TaskSync.Enums;

namespace TaskSync.Models.Dto
{
    public class TaskDto
    {
        public int Id { get; set; }
        public required string Title { get; set; }
        public int? CreatorId { get; set; }
        public int? AssigneeId { get; set; }
        public TASK_STATUS Status { get; set; }
        public DateTime LastModified { get; set; }
    }
}
