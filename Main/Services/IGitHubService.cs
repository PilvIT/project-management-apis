using Core.Features.GitHubApp;

namespace Main.Services;

public interface IGitHubService
{
    public GitHubAuthorization Authorization { get; }

    public GitHubUserApiClient GetUserApiClient(string accessToken);
}