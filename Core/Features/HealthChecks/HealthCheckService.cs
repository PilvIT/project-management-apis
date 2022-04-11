using Core.Features.HealthChecks.ViewModels;
using Core.Models;

namespace Core.Features.HealthChecks;

public class HealthCheckService : IHealthCheckService
{
    private readonly AppDbContext _dbContext;
    
    public HealthCheckService(AppDbContext dbContext)
    {
        _dbContext = dbContext;
    }

    /// <inheritdoc />
    public async Task<HealthCheck> AddHealthCheckUrlAsync(GitRepository repository, string url)
    {
        var instance = new HealthCheck
        {
            RepositoryId = repository.Id,
            Url = url
        };
        await _dbContext.HealthChecks.AddAsync(instance);
        await _dbContext.SaveChangesAsync();
        
        return instance;
    }

    /// <inheritdoc />
    public Task<HealthCheckStatus> CheckHealth(HealthCheck instance)
    {
        throw new NotImplementedException();
    }

    /// <inheritdoc />
    public async Task DeleteHealthCheckAsync(Guid id)
    {
        HealthCheck? instance = await _dbContext.HealthChecks.FindAsync(id);
        if (instance != null)
        {
            _dbContext.HealthChecks.Remove(instance);
            await _dbContext.SaveChangesAsync();
        }
    }

    /// <inheritdoc />
    public async Task<HealthCheck?> RetrieveAsync(Guid id) => await _dbContext.HealthChecks.FindAsync(id);
}
