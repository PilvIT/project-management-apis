using Core;
using Core.Features.Projects;
using Core.Features.Projects.ViewModels;
using Core.Models;
using Main.Injectables.Interfaces;
using Main.ViewModels;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Storage;

namespace Main.Api;

[Route("projects")]
public class ProjectApi : BaseApi
{
    private readonly AppDbContext _dbContext;
    private readonly IProjectService _service;

    public ProjectApi(AppDbContext dbContext, IAuthService authService, IProjectService projectService) : base(authService)
    {
        _dbContext = dbContext;
        _service = projectService;
    }

    [HttpPost]
    public async Task<ProjectDetail> CreateProject(ProjectCreateRequest request)
    {
        await using IDbContextTransaction transaction = await _dbContext.Database.BeginTransactionAsync();
        ProjectGroup group = request.Group ?? await _service.CreateProjectGroupAsync(request.GroupName);
        Project project = await _service.CreateProjectAsync(request.Name, group);
        await transaction.CommitAsync();

        return new ProjectDetail(project);
    }
    
    
    [HttpGet]
    public Paginated<ProjectListDetail> ListProjects()
    {
        IQueryable<ProjectListDetail> queryable = _dbContext.Projects
            .Include(p => p.Group)
            .Select(p => new ProjectListDetail
            {
                Id = p.Id,
                DisplayName = $"{p.Group!.Name} / {p.Name}"
            });

        return new Paginated<ProjectListDetail>(queryable);
    }

    [HttpGet("{id:guid}")]
    public ActionResult<ProjectDetail> RetrieveProject(Guid id)
    {
        try
        {
            Project project = _dbContext.Projects
                .Include(m => m.Group)
                .Include(m => m.GitRepositories)
                .ThenInclude(m => m.Technologies)
                .Single(m => m.Id == id);

            return new ProjectDetail(project);
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
