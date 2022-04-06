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
    public async Task<ActionResult<ProjectDetail>> RetrieveProject(Guid id)
    {
        Project? project = await _service.RetrieveProjectAsync(id);
        return project != null ? new ProjectDetail(project) : new NotFoundResult();
    }

    [HttpDelete("{id:guid}")]
    public ActionResult DeleteProject(Guid id)
    {
        _service.DeleteProjectAsync(id);
        return new OkObjectResult(new { });
    }
}
