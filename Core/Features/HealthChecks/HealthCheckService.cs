using Core.Extensions;
using Core.Features.HealthChecks.ViewModels;
using Core.Models;

namespace Core.Features.HealthChecks;

public class HealthCheckService : IHealthCheckService
{
    private readonly AppDbContext _dbContext;
    private readonly IHttpClientFactory _httpClientFactory;
    
    public HealthCheckService(AppDbContext dbContext, IHttpClientFactory httpClientFactory)
    {
        _dbContext = dbContext;
        _httpClientFactory = httpClientFactory;
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
    public async Task<HealthCheckStatus> CheckHealthAsync(HealthCheck instance)
    {
        HttpClient client = _httpClientFactory.CreateRetryClient();
        HttpResponseMessage response = await client.GetAsync(instance.Url);
        return new HealthCheckStatus(response.StatusCode);
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
