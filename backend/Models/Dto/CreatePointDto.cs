using TaskSync.Enums;

namespace TaskSync.Models.Dto
{
    public class CreatePointDto
    {
        public int TaskId { get; set; }
        public TASK_STATUS TaskStatus { get; set; }
        public required string UserId { get; set; }
    }
}
