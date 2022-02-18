using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace Core.Features.Projects.Models;

public class IssueLog : BaseModel
{
    public enum LogLevel
    {
        DEBUG = 2,
        INFO = 4,
        WARN = 6,
        ERROR = 8,
        FATAL = 10
    }
    
    [StringLength(500)]
    public string Message { get; set; } = null!;

    [Url]
    public string DetailLink { get; set; } = null!;
    
    public LogLevel Level { get; set; }
    public double CvssScore { get; set; }
    
    public DateTime CreatedAt { get; set; }
    public bool IsResolved { get; set; }
    
    [ForeignKey(nameof(GitRepository))]
    public Guid RepositoryId { get; set; }
    public GitRepository? Repository { get; set; }
}

public class IssueLogConfiguration : BaseModelConfiguration<IssueLog>
{
}