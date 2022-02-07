using System.ComponentModel.DataAnnotations;

namespace Core.Features.Projects.Models;

public class Technology
{
    [Key]
    public int Id { get; init; }
    public string Name { get; set; } = null!;
    public string? Icon { get; set; }
}
