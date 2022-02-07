using System.Collections.ObjectModel;
using Microsoft.EntityFrameworkCore;

namespace Core.Features.Projects.Models;

[Index(nameof(Name), IsUnique = true)]
public class ProjectGroup : BaseModel
{
    public string Name { get; set; } = null!;
    
    public Collection<Project>? Projects { get; set; }
}

public class ProjectGroupConfiguration : BaseModelConfiguration<ProjectGroup>
{
}