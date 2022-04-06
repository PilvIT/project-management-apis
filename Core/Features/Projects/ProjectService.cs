using Core.Models;

namespace Core.Features.Projects;

public class ProjectService : IProjectService
{
    private AppDbContext _dbContext { get; set; }
    
    public ProjectService(AppDbContext dbContext)
    {
        _dbContext = dbContext;
    }
    
    /// <inheritdoc />
    public async Task<Project> CreateProjectAsync(string name, ProjectGroup group)
    {
        var instance = new Project { Name = name, Group = group };
        await _dbContext.Projects.AddAsync(instance);
        await _dbContext.SaveChangesAsync();
        return instance;
    }

    /// <inheritdoc />
    public async Task<ProjectGroup> CreateProjectGroupAsync(string name)
    {
        var instance = new ProjectGroup { Name = name };
        await _dbContext.ProjectGroups.AddAsync(instance);
        await _dbContext.SaveChangesAsync();
        return instance;
    }
}