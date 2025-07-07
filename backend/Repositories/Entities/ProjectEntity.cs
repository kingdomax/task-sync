using System.ComponentModel.DataAnnotations.Schema;

namespace TaskSync.Repositories.Entities
{
    [Table("projects")]
    public class ProjectEntity
    {
        [Column("id")] public int Id { get; set; }
        [Column("name")] public required string Name { get; set; }
    }
}
