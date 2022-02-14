using System.Text.Json;
using System.Text.Json.Serialization;
using Core.Features.Projects.Models;

namespace Core.Features.Projects;

/// <summary>
/// Loads the artifact data to database
/// </summary>
public class ArtifactLoader
{
    private readonly string _artifactPath;
    private readonly AppDbContext _dbContext;

    public ArtifactLoader(string path, AppDbContext dbContext)
    {
        _artifactPath = path;
        _dbContext = dbContext;
    }

    public async Task LoadToDbAsync(GitRepository repository)
    {
        await LoadNpmPackageJsonAsync(repository);
        await _dbContext.SaveChangesAsync();
    }

    private async Task LoadNpmPackageJsonAsync(GitRepository repository)
    {
        // TODO: Support multiple package.json -files.
        var filePath = $"{_artifactPath}/package.json";
        if (!File.Exists(filePath))
        {
            return;
        }

        string data = await File.ReadAllTextAsync(filePath);
        var packageJson = JsonSerializer.Deserialize<NpmPackageJson>(data)!;
        
        Dependency? dependency = _dbContext.Dependencies
            .Where(d => d.GitRepositoryId == repository.Id)
            .SingleOrDefault(d => d.Path == filePath);
        if (dependency == null)
        {
            dependency = new Dependency
            {
                Path = filePath,
                Content = packageJson.Dependencies,
                GitRepositoryId = repository.Id
            };
        }
        dependency.Content = packageJson.Dependencies;
        _dbContext.Dependencies.Add(dependency);
    }
}

internal class NpmPackageJson
{
    [JsonPropertyName("dependencies")]
    public Dictionary<string, string> Dependencies { get; set; } = null!;
}