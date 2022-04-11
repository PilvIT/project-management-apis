using Core.Features.HealthChecks.ViewModels;
using Core.Models;

namespace Core.Features.HealthChecks;

public interface IHealthCheckService
{
    /// <summary>
    /// Adds an health check url to a repository.
    /// </summary>
    /// <param name="repository">an instance of the repository</param>
    /// <param name="url">url for health check</param>
    /// <exception cref="ArgumentException">a repository with given id does not exist</exception>
    /// <returns>instance of HealthCheck</returns>
    public Task<HealthCheck> AddHealthCheckUrlAsync(GitRepository repository, string url);
    
    /// <summary>
    /// Calls the health check endpoints and check its status. 
    /// </summary>
    /// <param name="instance">of the health check</param>
    /// <returns>the status of health check</returns>
    public Task<HealthCheckStatus> CheckHealthAsync(HealthCheck instance);

    public Task DeleteHealthCheckAsync(Guid id);
    public Task<HealthCheck?> RetrieveAsync(Guid id);
}
