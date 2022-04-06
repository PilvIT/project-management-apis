using Core.Models;
using Microsoft.EntityFrameworkCore;

namespace Core.Features.Projects;

public class ProjectService : IProjectService
{
    private readonly AppDbContext _dbContext;
    
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

    public async Task DeleteProjectAsync(Guid id)
    {
        Project? project = await _dbContext.Projects.FindAsync(id);
        if (project != null)
        {
            _dbContext.Projects.Remove(project);
            await _dbContext.SaveChangesAsync();
        }
    }
    
    public async Task<Project?> RetrieveProjectAsync(Guid id)
    {
        Project? project = await _dbContext.Projects
            .Include(m => m.Group)
            .Include(m => m.GitRepositories)
            .ThenInclude(m => m.Technologies)
            .SingleOrDefaultAsync(m => m.Id == id);

        return project;
    }
}
