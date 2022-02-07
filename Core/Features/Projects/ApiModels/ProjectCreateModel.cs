using System.ComponentModel.DataAnnotations;
using Microsoft.EntityFrameworkCore;

namespace Core.Features.Projects.ApiModels;

public class ProjectCreateModel : IValidatableObject
{
    [Required]
    [StringLength(200, MinimumLength = 1)]
    public string Group { get; set; } = null!;

    [Required]
    [StringLength(200, MinimumLength = 1)]
    public string Name { get; set; } = null!;
    
    public IEnumerable<ValidationResult> Validate(ValidationContext validationContext)
    {
        // TODO: Validate that project doesn't exist in the group.
        var dbContext = (AppDbContext) validationContext.GetService(typeof(AppDbContext))!;
            
        
        yield return new ValidationResult(
            "Project already exists in the group",
            new[] { nameof(Group), nameof(Name) });
    }
}