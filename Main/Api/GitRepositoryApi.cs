using Core;
using Core.Features.GitHubApp;
using Core.Features.GitHubApp.ApiModels;
using Core.Features.Projects.ApiModels;
using Core.Features.Projects.Models;
using Main.Injectables.Interfaces;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Hosting.Internal;

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

    [HttpGet("{id:guid}/refresh")]
    public async Task<ActionResult> RefreshRepositoryInfo(Guid id)
    {
        GitRepository? repository = _dbContext.GitRepositories.Find(id);
        if (repository == null)
        {
            return new NotFoundResult();
        }

        var client = new GitHubRepositoryApiClient(_conf.GetGitHubAppName(), GitHubTokens);
        return new ObjectResult(await client.GetLatestArtifacts(repository.Url, _artifactDirectory));
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
}
