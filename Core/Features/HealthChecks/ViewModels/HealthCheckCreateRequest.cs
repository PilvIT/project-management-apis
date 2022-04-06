using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;
using Core.Models;

namespace Core.Features.HealthChecks.ViewModels;

public class HealthCheckCreateRequest : IValidatableObject
{
    public Guid RepositoryId { get; set; }

    [JsonIgnore]
    public GitRepository? Repository { get; set; }
    
    [Url]
    [StringLength(500)]
    public string Url { get; set; } = null!;


    public IEnumerable<ValidationResult> Validate(ValidationContext validationContext)
    {
        var dbContext = (AppDbContext) validationContext.GetService(typeof(AppDbContext))!;

        GitRepository? repository =  dbContext.GitRepositories.Find(RepositoryId);
        if (repository == null)
        {
            yield return new ValidationResult($"Repository with id {RepositoryId} is not found!", new[] { nameof(RepositoryId) });
        }
        else
        {
            Repository = repository;
        }
    }
}
