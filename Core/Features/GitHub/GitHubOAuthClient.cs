using System.Net.Http.Json;
using Core.Features.GitHub.Interfaces;
using Core.Features.GitHub.ViewModels;
using Microsoft.AspNetCore.WebUtilities;
using Microsoft.Net.Http.Headers;

namespace Core.Features.GitHub;

/// <summary>
/// Implements a web application flow for GitHub Authorization.
/// <see href="https://docs.github.com/en/developers/apps/building-github-apps/identifying-and-authorizing-users-for-github-apps#web-application-flow">Official Guide</see>
/// </summary>
public class GitHubOAuthClient : IGitHubOAuthClient
{
    private const string Host = "https://github.com";
    
    private readonly string _clientId;
    private readonly string _clientSecret;
    private readonly HttpClient _httpClient;
    
    public GitHubOAuthClient(string clientId, string clientSecret)
    {
        _clientId = clientId;
        _clientSecret = clientSecret;
        _httpClient = new HttpClient();
        _httpClient.BaseAddress = new Uri(Host);
        _httpClient.DefaultRequestHeaders.Add(HeaderNames.Accept, "application/json");
    }

    /// <inheritdoc />
    public GitHubOAuthInitResponse GetUrl(string redirectUri)
    {
        var state = Guid.NewGuid().ToString();
        var parameters = new Dictionary<string, string>
        {
            { "client_id", _clientId },
            { "redirect_uri", redirectUri },
            { "state", state },
            { "allow_signup", "false" }
        };

        var url = QueryHelpers.AddQueryString($"{Host}/login/oauth/authorize", parameters);
        return new GitHubOAuthInitResponse(url: url, state: state);
    }

    /// <inheritdoc />
    public async Task<GitHubTokenDetail> ExchangeTokenAsync(string code, string redirectUri, string state)
    {
        var requestData = new Dictionary<string, string>
        {
            { "client_id", _clientId },
            { "client_secret", _clientSecret },
            { "code", code },
            { "redirect_uri", redirectUri },
            { "state", state }
        };

        HttpResponseMessage response = await _httpClient.PostAsJsonAsync("/login/oauth/access_token", requestData);
        return (await response.Content.ReadFromJsonAsync<GitHubTokenDetail>())!;
    }
}
