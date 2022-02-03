using System.Text.Json.Serialization;

namespace Core.Features.GitHubApp.JsonModels;

public class GitHubTokenExchangeResponse
{
    [JsonPropertyName("access_token")]
    public string AccessToken { get; set; }
    
    [JsonPropertyName("refresh_token")]
    public string RefreshToken { get; set; }
}