using Core.Features.GitHubApp;

namespace Main.Injectables.Interfaces;

public interface IGitHubService
{
    public IGitHubAuthorization Authorization { get; }

    public IGitHubUserApiClient GetUserApiClient(string accessToken);
}