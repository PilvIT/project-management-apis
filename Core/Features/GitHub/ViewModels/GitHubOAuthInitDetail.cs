namespace Core.Features.GitHub.ViewModels;

public class GitHubOAuthInitResponse
{
    public string Url { get; }
    public string State { get; }
    
    public GitHubOAuthInitResponse(string url, string state)
    {
        State = state;
        Url = url;
    }
}
