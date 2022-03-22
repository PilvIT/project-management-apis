using System.Text.Json.Serialization;

namespace Core.Features.GitHub.ViewModels;

public class GitHubUserDetail
{
    public long Id { get; set; }
    public string Name { get; set; } = null!;

    [JsonPropertyName("html_url")]
    public string Url { get; set; } = null!;
}
