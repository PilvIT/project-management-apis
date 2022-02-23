using System.Text.Json.Serialization;
using Microsoft.EntityFrameworkCore;

namespace Core.Models;

[Index(nameof(Name), IsUnique = true)]
public class ProjectGroup : BaseModel
{
    public string Name { get; set; } = null!;
    
    [JsonIgnore]
    public List<Project>? Projects { get; set; }
}

public class ProjectGroupConfiguration : BaseModelConfiguration<ProjectGroup>
{
}