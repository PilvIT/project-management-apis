namespace Core.Features.GitHubApp.ApiModels;

public class GitHubAuthorizationUrl
{
    public string Url { get; }
    public string State { get; }
    
    public GitHubAuthorizationUrl(string url, string state)
    {
        State = state;
        Url = url;
    }
}