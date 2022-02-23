using Core.Features.GitHub.ViewModels;
using Core.Models;

namespace Core.Features.GitHub.Interfaces;

public interface IGitHubRepositoryApiClient
{
    public Task<string?> GetLatestArtifacts(GitRepository repository, string targetDirectory);
    public Task<GitHubRepositoryResponse> GetRepositoryDetailAsync(GitRepository repository);
}
