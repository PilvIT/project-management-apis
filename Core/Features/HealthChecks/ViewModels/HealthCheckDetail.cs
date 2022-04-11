using Core.Models;

namespace Core.Features.HealthChecks.ViewModels;

public class HealthCheckDetail
{
    public Guid Id { get; set; }
    public Guid Repository { get; set; }
    public string Url { get; set; } = null!;

    public HealthCheckDetail()
    {
    }

    public HealthCheckDetail(HealthCheck model)
    {
        Id = model.Id;
        Repository = model.RepositoryId;
        Url = model.Url;
    }

    public HealthCheckDetail(HealthCheck model, HealthCheckStatus status)
    {
        
    }
}
