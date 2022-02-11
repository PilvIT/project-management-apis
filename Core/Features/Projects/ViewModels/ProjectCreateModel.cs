using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;
using Core.Features.Projects.Models;
using Microsoft.EntityFrameworkCore;

namespace Core.Features.Projects.ApiModels;

public class ProjectCreateModel : IValidatableObject
{
    [Required]
    [JsonPropertyName("group")]
    [StringLength(200, MinimumLength = 1)]
    public string GroupName { get; set; } = null!;

    [Required]
    [StringLength(200, MinimumLength = 1)]
    public string Name { get; set; } = null!;
    
    [JsonIgnore]
    public ProjectGroup? Group { get; set; }
    
    public IEnumerable<ValidationResult> Validate(ValidationContext validationContext)
    {
        var dbContext = (AppDbContext) validationContext.GetService(typeof(AppDbContext))!;
        Group = dbContext.ProjectGroups.FirstOrDefault(m => m.Name == GroupName);
        
        Project? project = dbContext.Projects
            .Include(m => m.Group)
            .FirstOrDefault(m => m.Group!.Name == GroupName && m.Name == Name);
        
        if (project != null)
        {
            yield return new ValidationResult(
                $"Project '{Name}' already exists in the group '{GroupName}'!",
                new[] { nameof(GroupName), nameof(Name) });
        }
    }
}
