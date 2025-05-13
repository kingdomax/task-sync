using System.ComponentModel.DataAnnotations;

namespace TaskSync.Models.Dto
{
    public class AddTaskRequest
    {
        [Required]
        public required string Title { get; set; }
        public int? AssigneeId { get; set; }
        public int ProjectId { get; set; }
    }
}
