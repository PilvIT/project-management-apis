using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Core.Features.Projects.Models;

public class GitRepository : BaseModel
{
    [StringLength(150, MinimumLength = 1)]
    public string Name { get; set; } = null!;
    
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
