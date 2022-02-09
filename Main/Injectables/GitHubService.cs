using Core.Features.GitHubApp;
using Main.Injectables.Interfaces;

namespace Main.Injectables;

public class GitHubService : IGitHubService
{
    private readonly IConfiguration _conf;
    
    public IGitHubAuthorization Authorization { get; }
    
    public GitHubService(IConfiguration conf)
    {
        _conf = conf;
        Authorization = new GitHubAuthorization(conf.GetGitHubClientId(), conf.GetGitHubClientSecret());
    }
    
    public IGitHubUserApiClient GetUserApiClient(string accessToken)
    {
        return new GitHubUserApiClient(appName: _conf.GetGitHubAppName(), accessToken: accessToken);
    } 
}
