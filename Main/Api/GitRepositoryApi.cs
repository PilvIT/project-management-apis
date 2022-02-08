using Core;
using Core.Features.Projects.ApiModels;
using Core.Features.Projects.Models;
using Main.Injectables.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace Main.Api;

[Route("git-repositories")]
public class GitRepositoryApi : ApiBase
{
    private readonly AppDbContext _dbContext;
    
    public GitRepositoryApi(AppDbContext dbContext, IAuth auth) : base(auth)
    {
        _dbContext = dbContext;
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
