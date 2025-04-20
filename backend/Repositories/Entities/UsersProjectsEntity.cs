using System.ComponentModel.DataAnnotations.Schema;

namespace TaskSync.Repositories.Entities
{
    [Table("users_projects")]
    public class UsersProjectsEntity
    {
        [Column("user_id")] public int UserId { get; set; }
        [Column("project_id")] public int ProjectId { get; set; }
    }
}
