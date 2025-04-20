using System.ComponentModel.DataAnnotations;
using TaskSync.Enums;

public class UpdateTaskRequest : IValidatableObject
{
    [Required]
    public required string StatusRaw { get; set; }

    public IEnumerable<ValidationResult> Validate(ValidationContext validationContext)
    {
        if (!Enum.TryParse(typeof(TASK_STATUS), StatusRaw, true, out _))
        {
            yield return new ValidationResult($"Invalid status: {StatusRaw}");
        }
    }
}