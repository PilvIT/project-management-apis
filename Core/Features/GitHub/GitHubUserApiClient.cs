using System.Net.Http.Headers;
using System.Net.Http.Json;
using Core.Features.GitHub.Interfaces;
using Core.Features.GitHub.ViewModels;
using Microsoft.Net.Http.Headers;

namespace Core.Features.GitHub;

public class GitHubUserApiClient : IGitHubUserApiClient
{
    private const string Host = "https://api.github.com";
    private readonly HttpClient _httpClient;

    public GitHubUserApiClient(string appName, string accessToken)
    {
        _httpClient = new HttpClient();
        _httpClient.BaseAddress = new Uri(Host);
        _httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("token", accessToken);
        _httpClient.DefaultRequestHeaders.Add(HeaderNames.Accept, "application/vnd.github.v3+json");
        _httpClient.DefaultRequestHeaders.Add(HeaderNames.UserAgent, appName);
    }

    /// <inheritdoc />
    public async Task<GitHubUserDetail> GetUserAsync()
    {
        HttpResponseMessage response = await _httpClient.GetAsync("/user");
        return (await response.Content.ReadFromJsonAsync<GitHubUserDetail>())!;
    }
    
    // TODO: Token refresh
}
