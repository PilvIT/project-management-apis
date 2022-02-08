using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;

namespace Core.Features.Projects.ApiModels;

public class GitRepositoryCreateModel
{
    [Required]
    [JsonPropertyName("project")]
    public Guid ProjectId { get; set; }
    
    [Required]
    [Url]
    [StringLength(400, MinimumLength = 10)]
    public string Url { get; set; } = null!;
}
