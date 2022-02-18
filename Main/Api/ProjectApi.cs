using Core;
using Core.Features.Projects.ApiModels;
using Core.Features.Projects.Models;
using Core.Features.Projects.ViewModels;
using Main.Injectables.Interfaces;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
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

    [HttpPost]
    public ProjectDetailModel CreateProject(ProjectCreateModel request)
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

        return new ProjectDetailModel
        {
            Id = project.Id,
            Name = project.Name,
            Group = project.Group.Name,
            Repositories = new List<GitRepository>()
        };
    }
    
    
    [HttpGet]
    public List<ProjectListDetail> ListProjects()
    {
        return _dbContext.Projects
            .Include(p => p.Group)
            .Select(p => new ProjectListDetail
            {
                Id = p.Id,
                DisplayName = $"{p.Group!.Name} / {p.Name}"
            }).ToList();
    }

    [HttpGet("{id:guid}")]
    public ActionResult<ProjectDetailModel> RetrieveProject(Guid id)
    {
        try
        {
            Project project = _dbContext.Projects
                .Include(m => m.Group)
                .Include(m => m.GitRepositories)
                .ThenInclude(m => m.Technologies)
                .Single(m => m.Id == id);
            
            return new ProjectDetailModel
            {
                Id = project.Id,
                Name = project.Name,
                Group = project.Group!.Name,
                Repositories = project.GitRepositories.ToList()
            };
        }
        catch (InvalidOperationException)
        {
            // The project does not exist
            return new NotFoundResult();
        }
    }

    [HttpDelete("{id:guid}")]
    public ActionResult DeleteProject(Guid id)
    {
        Project? project = _dbContext.Projects.Find(id);
        if (project != null)
        {
            _dbContext.Projects.Remove(project);
            _dbContext.SaveChanges();
        }

        return new OkObjectResult(new { });
    }
}
