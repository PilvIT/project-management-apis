using Core;
using Core.Features.GitHubApp;
using Core.Features.GitHubApp.ApiModels;
using Core.Features.Projects;
using Core.Features.Projects.ApiModels;
using Core.Features.Projects.Models;
using Core.Features.Projects.ViewModels;
using Core.Features.VulnerabilityManagement;
using Core.Features.VulnerabilityManagement.Models;
using Main.ApiModels;
using Main.Injectables.Interfaces;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace Main.Api;

[Route("git-repositories")]
public class GitRepositoryApi : ApiBase
{
    private readonly AppDbContext _dbContext;
    private readonly IConfiguration _conf;

    private readonly string _artifactDirectory;
    
    public GitRepositoryApi(AppDbContext dbContext, IAuth auth, IConfiguration conf, IWebHostEnvironment env) : base(auth)
    {
        _conf = conf;
        _dbContext = dbContext;
        
        _artifactDirectory = $"{env.ContentRootPath}/../{conf["Directories:GitHubArtifacts"]}";
    }
    
    [HttpGet]
    public async Task<PaginatedResponse<GitRepositoryListDetail>> ListRepositories([FromQuery] Guid projectId)
    {
        IQueryable<GitRepositoryListDetail> queryable = _dbContext.GitRepositories
            .Include(r => r.Technologies)
            .Where(r => r.ProjectId == projectId)
            .Select(r => new GitRepositoryListDetail
            {
                Id = r.Id,
                Name = r.Name,
                Technologies = r.Technologies!,
                Url = r.Url
            });
        
        return new PaginatedResponse<GitRepositoryListDetail>(queryable);
    } 
    
    [HttpGet("{id:guid}/refresh")]
    public async Task<ActionResult> RefreshRepositoryInfo(Guid id)
    {
        GitRepository? repository = await _dbContext.GitRepositories.FindAsync(id);
        if (repository == null)
        {
            return new NotFoundResult();
        }

        var client = new GitHubRepositoryApiClient(_conf.GetGitHubAppName(), GitHubTokens);
        GitHubRepositoryResponse repo = await client.GetRepositoryDetailAsync(repository);

        repository.Description = repo.Data.Repository.Description;
        repository.IsPublic = repo.Data.Repository.Visibility == "PUBLIC";

        var alertService = new VulnerabilityAlertService(_dbContext);
        alertService.BulkUpdate(repository, repo.Data.Repository.VulnerabilityAlerts.Nodes);
        
        
        /*var artifactPath = await client.GetLatestArtifacts(repository.Url, _artifactDirectory);
        if (artifactPath != null)
        {
            var artifactLoader = new ArtifactLoader(artifactPath, _dbContext);
            await artifactLoader.LoadToDbAsync(repository);
        }*/
        

        return new OkObjectResult(new {});
    }
    
    [HttpPost]
    public ActionResult<GitRepository> CreateGitRepository(GitRepositoryCreateModel request)
    {
        Project? project = _dbContext.Projects.Find(request.ProjectId);
        if (project == null)
        {
            return new NotFoundObjectResult(new Dictionary<string, string>
            {
                { "message", $"Project {request.ProjectId} not found!" }
            });
        }
        
        var repository = new GitRepository
        {
            Name = request.Url.Split("/").Last(),
            Url = request.Url,
            Technologies = new List<Technology>(),
            ProjectId = project.Id
        };
        _dbContext.GitRepositories.Add(repository);
        _dbContext.SaveChanges();

        return repository;
    }

    [HttpDelete("{id:guid}")]
    public async Task<ActionResult> DeleteGitRepository(Guid id)
    {
        var repository = new GitRepository { Id = id };
        _dbContext.Entry(repository).State = EntityState.Deleted;
        await _dbContext.SaveChangesAsync();

        return new OkObjectResult(new { });
    }
}
