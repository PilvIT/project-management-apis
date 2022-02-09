using System.Net.Http.Headers;
using System.Net.Http.Json;
using Core.Features.GitHubApp.ApiModels;
using Microsoft.Net.Http.Headers;

namespace Core.Features.GitHubApp;

public class GitHubUserApiClient : IGitHubUserApiClient
{
    private const string Host = "https://api.github.com";
    private readonly HttpClient _httpClient;

    public GitHubUserApiClient(string appName, string accessToken)
    {
        _httpClient = new HttpClient();
        _httpClient.BaseAddress = new Uri(Host);
        _httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("token", accessToken);
        _httpClient.DefaultRequestHeaders.Add(HeaderNames.Accept, "application/json");
        _httpClient.DefaultRequestHeaders.Add(HeaderNames.UserAgent, appName);
    }

    /// <inheritdoc />
    public async Task<GitHubUser> GetUserAsync()
    {
        HttpResponseMessage response = await _httpClient.GetAsync("/user");
        return (await response.Content.ReadFromJsonAsync<GitHubUser>())!;
    }
}
