using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text.Json.Serialization;
using Microsoft.EntityFrameworkCore;

namespace Core.Models;

[Index(nameof(GitHubId), IsUnique = true)]
public class Profile : BaseModel
{
    [ForeignKey(nameof(AppUser))]
    [JsonIgnore]
    [Required]
    public Guid AppUserId { get; set; }

    [Required]
    public long GitHubId { get; set; }

    [StringLength(200)]
    public string DisplayName { get; set; } = null!;
}

public class ProfileConfiguration : BaseModelConfiguration<Profile>
{
}
