using Core.Features.GitHubApp.ApiModels;

namespace Core.Features.GitHubApp;

public interface IGitHubAuthorization
{
    /// <summary>
    /// Returns an url to GitHub OAuth view.
    /// </summary>
    /// <param name="redirectUri">location to redirect user after authorization</param>
    public GitHubAuthorizationUrl GetUrl(string redirectUri);

    /// <summary>
    /// Exchange the code returned from GitHub to access token.
    /// </summary>
    /// <param name="code">from GitHub</param>
    /// <param name="redirectUri">location redirected from GitHub</param>
    /// <param name="state">state received assigned to uri</param>
    /// <returns>access and refresh tokens to GitHub</returns>
    public Task<GitHubTokens> ExchangeTokenAsync(string code, string redirectUri, string state);
}
