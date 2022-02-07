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
    
    protected GitRepositoryApi(AppDbContext dbContext, IAuth auth) : base(auth)
    {
        _dbContext = dbContext;
    }

    [HttpPost]
    public GitRepository CreateGitRepository(GitRepositoryCreateModel request)
    {
        var repository = new GitRepository
        {
            Name = request.Name,
            Url = request.Url
        };
        _dbContext.GitRepositories.Add(repository);
        _dbContext.SaveChanges();

        return repository;
    }
}
