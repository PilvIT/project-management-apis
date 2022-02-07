using System.ComponentModel.DataAnnotations;

namespace Core.Features.Projects.Models;

public class GitRepository : BaseModel
{
    [StringLength(150, MinimumLength = 1)]
    public string Name { get; set; } = null!;
    
    [StringLength(400, MinimumLength = 1)]
    public string Url { get; set; } = null!;
    public List<Technology>? Technologies { get; set; }
}

public class GitRepositoryConfiguration : BaseModelConfiguration<GitRepository>
{
}