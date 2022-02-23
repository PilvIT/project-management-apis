using Core.Features.GitHub;
using Core.Features.GitHub.Interfaces;
using Main.Injectables.Interfaces;

namespace Main.Injectables;

public class GitHubService : IGitHubService
{
    private readonly IConfiguration _conf;
    
    public IGitHubOAuthClient OAuthClient { get; }
    
    public GitHubService(IConfiguration conf)
    {
        _conf = conf;
        OAuthClient = new GitHubOAuthClient(conf.GetGitHubClientId(), conf.GetGitHubClientSecret());
    }
    
    public IGitHubUserApiClient GetUserApiClient(string accessToken)
    {
        return new GitHubUserApiClient(appName: _conf.GetGitHubAppName(), accessToken: accessToken);
    } 
}
