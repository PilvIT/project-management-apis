using Core.Features.GitHub.ViewModels;

namespace Core.Features.GitHub.Interfaces;

public interface IGitHubUserApiClient
{
    /// <summary>
    /// Returns user details from GitHub.
    /// </summary>
    public Task<GitHubUserDetail> GetUserAsync();
}
