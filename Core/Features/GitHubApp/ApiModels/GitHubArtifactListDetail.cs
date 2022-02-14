using System.Text.Json.Serialization;

namespace Core.Features.GitHubApp.ApiModels;

public class GitHubArtifactListDetail
{
    public int Id { get; set; }
    
    [JsonPropertyName("node_id")]
    public string NodeId { get; set; } = null!;
    
    public string Name { get; set; } = null!;
    
    [JsonPropertyName("created_at")]
    public DateTime CreatedAt { get; set; }
    
    [JsonPropertyName("archive_download_url")]
    public string DownloadUrl { get; set; } = null!;
    
    public bool Expired { get; set; }
}
