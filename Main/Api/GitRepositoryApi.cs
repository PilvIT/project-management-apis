﻿using Core;
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

    public GitRepositoryApi(AppDbContext dbContext, IAuth auth, IConfiguration conf, IWebHostEnvironment env) :
        base(auth)
    {
        _conf = conf;
        _dbContext = dbContext;

        _artifactDirectory = $"{env.ContentRootPath}/../{conf["Directories:GitHubArtifacts"]}";
    }

    [HttpGet]
    public PaginatedResponse<GitRepositoryListDetail> ListRepositories(
        [FromQuery] Guid? projectId,
        [FromQuery] bool? hasIssues)
    {
        IQueryable<GitRepository> queryable = _dbContext.GitRepositories
            .Include(r => r.Technologies)
            .Include(r => r.IssueLogs);

        if (hasIssues == true)
        {
            queryable = queryable.Where(g => g.IssueLogs!.Count > 0);
        }

        if (projectId != null)
        {
            queryable = queryable.Where(r => r.ProjectId == projectId);
        }

        IQueryable<GitRepositoryListDetail> repositories = queryable.Select(r => new GitRepositoryListDetail
        {
            Id = r.Id,
            Name = r.Name,
            Technologies = r.Technologies!,
            Issues = r.IssueLogs!,
            Description = r.Description!,
            Url = r.Url,
            ProjectId = r.ProjectId
        });

        return new PaginatedResponse<GitRepositoryListDetail>(repositories);
    }

    /// <summary>
    /// Manually refresh the repository details from GitHub
    /// </summary>
    /// <param name="id"></param>
    /// <returns></returns>
    [HttpPost("{id:guid}/refresh")]
    public async Task<ActionResult> RefreshRepository(Guid id)
    {
        GitRepository? repository = await _dbContext.GitRepositories.FindAsync(id);
        if (repository == null)
        {
            return new NotFoundResult();
        }

        var client = new GitHubRepositoryApiClient(_conf.GetGitHubAppName(), GitHubTokens);
        var refreshService = new GitRepositoryRefreshService(
            dbContext: _dbContext,
            repository: repository,
            apiClient: client);
        await refreshService.RefreshAsync(_artifactDirectory);
        return new OkObjectResult(new { });
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