using System.Net.Http.Headers;
using System.Net.Http.Json;
using Core.Exceptions;
using Core.Features.GitHubApp.ApiModels;

namespace Core.Features.GitHubApp;

public class GitHubUserApiClient
{
    private const string Host = "https://api.github.com";
    private readonly HttpClient _httpClient;

    public GitHubUserApiClient(string accessToken)
    {
        _httpClient = new HttpClient();
        _httpClient.BaseAddress = new Uri(Host);
        _httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("token", accessToken);
    }

    public async Task<GitHubUser> GetUserAsync()
    {
        HttpResponseMessage response = await _httpClient.GetAsync("/user");

        if (response.IsSuccessStatusCode)
        {
            return (await response.Content.ReadFromJsonAsync<GitHubUser>())!;
        }
        
        // TODO: Log the error
        throw new ApiException("");
    }
}
