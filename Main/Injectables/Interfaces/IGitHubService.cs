using Core.Features.GitHub;
using Core.Features.GitHub.Interfaces;

namespace Main.Injectables.Interfaces;

public interface IGitHubService
{
    public IGitHubOAuthClient OAuthClient { get; }

    public IGitHubUserApiClient GetUserApiClient(string accessToken);
}