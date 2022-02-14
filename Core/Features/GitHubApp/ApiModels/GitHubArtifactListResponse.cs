using System.Text.Json.Serialization;

namespace Core.Features.GitHubApp.ApiModels;

public class GitHubArtifactListResponse
{
    [JsonPropertyName("total_count")]
    public int Count { get; set; }

    public List<GitHubArtifactListDetail> Artifacts { get; set; } = null!;
}