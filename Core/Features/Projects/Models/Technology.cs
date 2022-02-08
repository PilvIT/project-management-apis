using System.ComponentModel.DataAnnotations;

namespace Core.Features.Projects.Models;

public class Technology
{
    [Key]
    public int Id { get; init; }
    
    [StringLength(150, MinimumLength = 1)]
    public string Name { get; set; } = null!;

    [StringLength(150, MinimumLength = 1)]
    public string? Icon { get; set; }
    
    public List<GitRepository>? GitRepositories { get; set; }
}
