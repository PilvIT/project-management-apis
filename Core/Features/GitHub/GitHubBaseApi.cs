using System.Net.Http.Headers;
using Core.Features.GitHub.ViewModels;
using Microsoft.Net.Http.Headers;

namespace Core.Features.GitHub;

/// <summary>
/// Handle http client creation.
/// </summary>
public class GitHubBaseApi
{
    protected const string Host = "https://api.github.com";

    private readonly string _userAgent;
    private readonly GitHubTokenResponse _tokenResponse;

    protected GitHubBaseApi(string userAgent, GitHubTokenResponse tokenResponse)
    {
        _tokenResponse = tokenResponse;
        _userAgent = userAgent;
    }

    protected HttpClient CreateHttpClient()
    {
        var client = new HttpClient();
        client.BaseAddress = new Uri(Host);
        client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("token", _tokenResponse.AccessToken);
        client.DefaultRequestHeaders.Add(HeaderNames.Accept, "application/vnd.github.v3+json");
        client.DefaultRequestHeaders.Add(HeaderNames.UserAgent, _userAgent);
        return client;
    }
}