using Core.Models;

namespace Core.Features.Projects.ViewModels;

public class ProjectDetail
{
    public Guid Id { get; set; }
    public string Name { get; set; }
    public string Group { get; set; }

    public List<GitRepository> Repositories { get; set; }

    public ProjectDetail()
    {
    }
    
    public ProjectDetail(Project model)
    {
        Id = model.Id;
        Name = model.Name;
        Group = model.Group!.Name;
        Repositories = model.GitRepositories.ToList();
    }
}