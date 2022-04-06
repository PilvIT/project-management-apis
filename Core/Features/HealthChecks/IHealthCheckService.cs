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

    public Task DeleteHealthCheckAsync(Guid id);
}