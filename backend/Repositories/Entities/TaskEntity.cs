using System.ComponentModel.DataAnnotations.Schema;
using TaskSync.Enums;

namespace TaskSync.Repositories.Entities
{
    [Table("tasks")]
    public class TaskEntity
    {
        [Column("id")] public int Id { get; set; }
        [Column("title")] public required string Title { get; set; }
        [Column("assignee_id")] public int? AssigneeId { get; set; }
        [Column("project_id")] public int ProjectId { get; set; }
        [Column("status")] public required string StatusRaw { get; set; } = "backlog";
        [Column("last_modified")] public DateTime LastModified { get; set; }

        [NotMapped]
        public TASK_STATUS Status
        {
            get => Enum.Parse<TASK_STATUS>(StatusRaw, ignoreCase: true); // Read from DB
            set => StatusRaw = value.ToString().ToLower();               // Write to DB
        }
    }
}
