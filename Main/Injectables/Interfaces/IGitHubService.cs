using Core.Features.GitHubApp;

namespace Main.Injectables.Interfaces;

public interface IGitHubService
{
    public GitHubAuthorization Authorization { get; }

    public GitHubUserApiClient GetUserApiClient(string accessToken);
}