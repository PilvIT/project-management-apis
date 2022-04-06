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
}
