using System.ComponentModel.DataAnnotations;

namespace TaskSync.Controllers.TempDto
{
    public class QueryStringDto
    {
        [Required]
        [MinLength(4, ErrorMessage = "Name must be at least 4 characters")]
        public string Name { get; set; }

        [Required]
        [Range(1, 5, ErrorMessage = "Priority must be between 1 and 5")]
        public int? Priority { get; set; }

        [Required]
        public string Email { get; set; }
    }
}
