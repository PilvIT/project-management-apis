using Core.Models;

namespace Core.Features.Projects.ViewModels;

public class ProjectDetail
{
    public Guid Id { get; set; }
    public string Name { get; set; } = null!;
    public string Group { get; set; } = null!;

    public List<GitRepository> Repositories { get; set; } = null!;
}