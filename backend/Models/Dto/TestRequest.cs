using System.ComponentModel.DataAnnotations;

namespace TaskSync.Models.Dto
{
    public class TestRequest
    {
        public int Id { get; set; }

        [Required]
        public string? Name { get; set; }
    }
}
