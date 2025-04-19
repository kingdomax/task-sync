using System.ComponentModel.DataAnnotations.Schema;

namespace TaskSync.Repositories.Entity
{
    [Table("users")]
    public class UserEntity
    {
        [Column("id")] public int Id { get; set; }
        [Column("username")] public required string Username { get; set; }
        [Column("firstname")] public required string Firstname { get; set; }
        [Column("lastname")] public required string Lastname { get; set; }
        [Column("email")] public required string Email { get; set; }
        [Column("password")] public required string Password { get; set; }
    }
}
