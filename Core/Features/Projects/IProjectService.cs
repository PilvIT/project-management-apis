using Core.Models;

namespace Core.Features.Projects;

public interface IProjectService
{
    public Task<Project> CreateProjectAsync(string name, ProjectGroup group);
    public Task<ProjectGroup> CreateProjectGroupAsync(string name);
    public Task DeleteProjectAsync(Guid id);
    public Task<Project?> RetrieveProjectAsync(Guid id);
}
