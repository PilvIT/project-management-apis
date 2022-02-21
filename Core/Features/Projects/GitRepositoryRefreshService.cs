using Core.Features.GitHubApp;
using Core.Features.GitHubApp.ApiModels;
using Core.Features.Projects.Models;
using Core.Features.VulnerabilityManagement;
using Microsoft.EntityFrameworkCore.Storage;

namespace Core.Features.Projects;

public class GitRepositoryRefreshService
{
    private readonly IGitHubRepositoryApiClient _apiClient;
    private readonly AppDbContext _dbContext;
    private readonly GitRepository _gitRepository;
    
    public GitRepositoryRefreshService(AppDbContext dbContext, GitRepository repository, IGitHubRepositoryApiClient apiClient)
    {
        _apiClient = apiClient;
        _dbContext = dbContext;
        _gitRepository = repository;
    }
    
    public async Task RefreshAsync(string artifactDirectory)
    {
        GitHubRepositoryResponse response = await _apiClient.GetRepositoryDetailAsync(_gitRepository);
        
        await using IDbContextTransaction transaction = await _dbContext.Database.BeginTransactionAsync();
        _gitRepository.Description = response.Data.Repository.Description;
        _gitRepository.IsPublic = response.Data.Repository.Visibility == "PUBLIC";
        
        await SyncVulnerabilityAlerts(response.Data.Repository.VulnerabilityAlerts.Nodes);
        await SyncArtifactsAsync(artifactDirectory);

        await _dbContext.SaveChangesAsync();
        await transaction.CommitAsync();
    }

    private async Task SyncVulnerabilityAlerts(List<VulnerabilityNodeGraphQL> vulnerabilities)
    {
        var alertService = new VulnerabilityAlertService(_dbContext);
        await alertService.UpdateAsync(_gitRepository, vulnerabilities);
    }

    private async Task SyncArtifactsAsync(string artifactDirectory)
    {
        var artifactPath = await _apiClient.GetLatestArtifacts(_gitRepository, artifactDirectory);
        if (artifactPath != null)
        {
            var artifactLoader = new ArtifactLoader(artifactPath, _dbContext);
            await artifactLoader.LoadToDbAsync(_gitRepository);
        }
    }
}
