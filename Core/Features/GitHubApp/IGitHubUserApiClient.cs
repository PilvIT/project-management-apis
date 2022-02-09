using Core.Features.GitHubApp.ApiModels;

namespace Core.Features.GitHubApp;

public interface IGitHubUserApiClient
{
    /// <summary>
    /// Returns user details from GitHub.
    /// </summary>
    public Task<GitHubUser> GetUserAsync();
}
