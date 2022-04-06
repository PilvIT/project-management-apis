using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Core.Models;

public class HealthCheck : BaseModel
{
    [StringLength(500)]
    public string Url { get; set; } = null!;
    
    [ForeignKey("RepositoryId")]
    public GitRepository? Repository { get; set; }
    public Guid RepositoryId { get; set; }
}

public class HealthCheckConfiguration : BaseModelConfiguration<HealthCheck>
{
}
