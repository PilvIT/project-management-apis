using System.ComponentModel.DataAnnotations.Schema;

namespace Core.Features.Projects.Models;

public class Project : BaseModel
{
    public string Name { get; set; } = null!;
    
    [ForeignKey(nameof(ProjectGroup))]
    public Guid GroupId { get; set; }
    public ProjectGroup? Group { get; set; }
}

public class ProjectConfiguration : BaseModelConfiguration<Project>
{
}
