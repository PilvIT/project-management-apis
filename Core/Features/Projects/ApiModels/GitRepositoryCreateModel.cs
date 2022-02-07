using System.ComponentModel.DataAnnotations;

namespace Core.Features.Projects.ApiModels;

public class GitRepositoryCreateModel
{
    [StringLength(400, MinimumLength = 1)]
    public string Name { get; set; } = null!;

    
    [Url]
    [StringLength(400, MinimumLength = 10)]
    public string Url { get; set; } = null!;
}
