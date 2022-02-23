using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;
using Microsoft.EntityFrameworkCore;

namespace Core.Models;

[Index(nameof(Name), IsUnique = true)]
public class Technology
{
    [Key]
    public int Id { get; init; }
    
    [JsonIgnore]
    [StringLength(150, MinimumLength = 1)]
    public string Name { get; set; } = null!;

    [JsonPropertyName("Name")]
    [StringLength(150, MinimumLength = 1)]
    public string DisplayName { get; set; } = null!;

    [StringLength(150, MinimumLength = 1)]
    public string? Icon { get; set; } = null!;
    
    public List<GitRepository>? GitRepositories { get; set; }
}
