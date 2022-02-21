using Core.Features.GitHubApp.ApiModels;
using Core.Features.Projects.Models;

namespace Core.Features.GitHubApp;

public interface IGitHubRepositoryApiClient
{
    public Task<string?> GetLatestArtifacts(GitRepository repository, string targetDirectory);
    public Task<GitHubRepositoryResponse> GetRepositoryDetailAsync(GitRepository repository);
}
