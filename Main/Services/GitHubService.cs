using Core.Features.GitHubApp;

namespace Main.Services;

public class GitHubService : IGitHubService
{
    public GitHubAuthorization Authorization { get; }
    
    public GitHubService(IConfiguration conf)
    {
        Authorization = new GitHubAuthorization(conf.GetGitHubClientId(), conf.GetGitHubClientSecret());
    }
}