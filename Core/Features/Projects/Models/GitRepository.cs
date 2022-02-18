using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Core.Features.Projects.Models;

public class GitRepository : BaseModel
{
    [StringLength(150, MinimumLength = 1)]
    public string Name { get; set; } = null!;
    public bool IsPublic { get; set; }
    
    [StringLength(1000)]
    public string? Description { get; set; }
    
    [StringLength(400, MinimumLength = 1)]
    public string Url { get; set; } = null!;
    public List<Technology>? Technologies { get; set; }
    public List<Dependency>? Dependencies { get; set; }
    
    [ForeignKey(nameof(Project))]
    public Guid ProjectId { get; set; }
}

public class GitRepositoryConfiguration : BaseModelConfiguration<GitRepository>
{
}
