using System.ComponentModel.DataAnnotations;

namespace Main.ViewModels;

public class AuthInitRequest
{
    [Required]
    [Url]
    public string RedirectUri { get; set; } = null!;
}
