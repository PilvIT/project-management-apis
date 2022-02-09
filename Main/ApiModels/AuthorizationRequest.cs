using System.ComponentModel.DataAnnotations;

namespace Main.ApiModels;

public class AuthorizationRequest
{
    [Required]
    [Url]
    public string RedirectUri { set; get; } = null!;
}
