using System.ComponentModel.DataAnnotations;

namespace TaskSync.Repositories.Entity
{
    public class UserEntity
    {
        public int Id { get; set; }
        public required string Username { get; set; }
        public required string Firstname { get; set; }
        public required string Lastname { get; set; }
        public required string Email { get; set; }
    }
}
