namespace Core.Features.Projects.ApiModels;

public class ProjectDetailModel
{
    public Guid Id { get; set; }
    public string Name { get; set; } = null!;
    public string Group { get; set; } = null!;
}