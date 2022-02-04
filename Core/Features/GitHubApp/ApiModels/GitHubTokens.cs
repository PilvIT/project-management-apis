﻿using System.Text.Json.Serialization;

namespace Core.Features.GitHubApp.ApiModels;

public class GitHubTokens
{
    [JsonPropertyName("access_token")]
    public string AccessToken { get; set; } = null!;
    
    [JsonPropertyName("refresh_token")]
    public string RefreshToken { get; set; } = null!;
}