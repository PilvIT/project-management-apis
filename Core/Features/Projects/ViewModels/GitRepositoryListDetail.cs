using Core.Models;

namespace Core.Features.Projects.ViewModels;

public class GitRepositoryListDetail
{
    public Guid Id { get; set; }
    public string Name { get; set; } = null!;
    public string Description { get; set; } = "Lorem ipsum dolor sit amet, consectetur adipiscing elit. Integer elit sapien, rutrum ac sem nec, rutrum interdum lectus. Donec pretium, erat in vestibulum scelerisque, neque augue tristique felis, eu congue magna leo in libero.";
    public string Url { get; set; } = null!;
    public List<Technology> Technologies { get; set; } = null!;
    public List<IssueLog> Issues { get; set; } = null!;
    
    public Guid ProjectId { get; set; }
}