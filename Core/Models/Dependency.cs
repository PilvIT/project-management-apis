using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace Core.Models;

[Index(nameof(Path), nameof(GitRepositoryId), IsUnique = true)]
public class Dependency : BaseModel
{
    /// <summary>
    /// There may exist multiple dependency files e.g. in monorepo or .NET solutions.
    /// </summary>
    [StringLength(1000)]
    public string Path { get; set; } = null!;

    [Column(TypeName = "jsonb")]
    public Dictionary<string, string> Content { get; set; } = null!;

    [ForeignKey(nameof(GitRepository))]
    public Guid GitRepositoryId { get; set; }
    public GitRepository? GitRepository { get; set; }
}
