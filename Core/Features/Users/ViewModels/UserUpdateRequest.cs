using System.ComponentModel.DataAnnotations;

namespace Core.Features.Users.ViewModels;

public class UserUpdateRequest
{
    [Required]
    [StringLength(200)]
    public string Name { get; set; } = null!;
   
    [Required]
    [StringLength(200)]
    public string Description { get; set; } = null!;
}
