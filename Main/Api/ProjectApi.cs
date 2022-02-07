using Core;
using Core.Features.Projects.ApiModels;
using Core.Features.Projects.Models;
using Main.Injectables.Interfaces;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore.Storage;

namespace Main.Api;

[Route("projects")]
public class ProjectApi : ApiBase
{
    private readonly AppDbContext _dbContext;
    
    public ProjectApi(AppDbContext dbContext, IAuth auth) : base(auth)
    {
        _dbContext = dbContext;
    }
    
    [HttpGet]
    public List<Project> GetProjects()
    {
        return _dbContext.Projects.ToList();
    }

    [HttpPost]
    public Project CreateProject(ProjectCreateModel request)
    {
        using IDbContextTransaction transaction = _dbContext.Database.BeginTransaction();
        ProjectGroup? group = request.Group;
        if (group == null)
        {
            group = new ProjectGroup { Name = request.GroupName };
            _dbContext.ProjectGroups.Add(group);
            _dbContext.SaveChanges();
        }

        var project = new Project
        {
            Name = request.Name,
            Group = group
        };
        _dbContext.Projects.Add(project);
        _dbContext.SaveChanges();
        transaction.Commit();

        return project;
    }
}
