using System.Diagnostics;
using System.Text.Json.Serialization;
using Core.Models;

namespace Core.Features.Users.ViewModels;

public class UserDetail
{
    public Guid Id { get; set; }
    public string Name { get; set; } = null!;
    public string Description { get; set; } = null!;
    public string Email { get; set; } = null!;
    
    [JsonPropertyName("github")]
    public string GitHub { get; set; } = null!;

    public UserDetail() { }

    public UserDetail(AppUser user)
    {
        Debug.Assert(user.Profile != null);
        Id = user.Id;
        Name = user.Profile.DisplayName;
        Description = user.Profile.Description;
        Email = user.Email;
        GitHub = user.Profile.GitHubUrl;
    }
}
