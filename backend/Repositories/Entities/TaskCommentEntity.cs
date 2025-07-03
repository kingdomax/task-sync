using System.ComponentModel.DataAnnotations.Schema;

using TaskSync.Enums;

namespace TaskSync.Repositories.Entities
{
    [Table("task_comments")]
    public class TaskCommentEntity
    {
        [Column("id")] public int Id { get; set; }
        [Column("task_id")] public int TaskId { get; set; }
        [Column("commenter_id")] public int? CommenterId { get; set; }
        [Column("type")] public required string TypeRaw { get; set; }
        [Column("created_at")] public DateTime CreatedAt { get; set; }
        [Column("comment_text")] public required string CommentText { get; set; }
        [Column("metadata")] public required Dictionary<string, object> MetaData { get; set; }

        [NotMapped]
        public TASK_COMMENT_TYPE Type
        {
            get => Enum.Parse<TASK_COMMENT_TYPE>(TypeRaw, ignoreCase: true); // Read from DB
            set => TypeRaw = value.ToString().ToLower();                     // Write to DB
        }
    }
}
