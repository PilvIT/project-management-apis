using System.Diagnostics;
using System.Text.Json;
using System.Text.Json.Serialization;
using Core.Models;

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
        await InferTechnologies(repository);
        await _dbContext.SaveChangesAsync();
    }

    private async Task InferTechnologies(GitRepository repository)
    {
        await _dbContext.Entry(repository).Collection(r => r.Dependencies!).LoadAsync();
        await _dbContext.Entry(repository).Collection(r => r.Technologies!).LoadAsync();
        Debug.Assert(repository.Dependencies != null);
        
        foreach (Dependency dependency in repository.Dependencies)
        {
            foreach (var (name, version) in dependency.Content)
            {
                Technology? t = _dbContext.Technologies.SingleOrDefault(t => t.Name == name);
                if (t != null && !repository.Technologies!.Exists(tx => tx.Name == t.Name))
                {
                    repository.Technologies.Add(t);
                }
            }
        }
    }

    private async Task LoadNpmPackageJsonAsync(GitRepository repository)
    {
        // TODO: Support multiple package.json -files.
        var filePath = $"{_artifactPath}/package.json";
        if (!File.Exists(filePath))
        {
            return;
        }

        var data = await File.ReadAllTextAsync(filePath);
        var packageJson = JsonSerializer.Deserialize<NpmPackageJson>(data)!;
        
        Dependency? dependency = _dbContext.Dependencies
            .Where(d => d.GitRepositoryId == repository.Id)
            .SingleOrDefault(d => d.Path == filePath);
        if (dependency == null)
        {
            _dbContext.Dependencies.Add(new Dependency
            {
                Path = filePath,
                Content = packageJson.Dependencies,
                GitRepositoryId = repository.Id
            });
        }
        else
        {
            dependency.Content = packageJson.Dependencies;
        }
    }
}

internal class NpmPackageJson
{
    [JsonPropertyName("dependencies")]
    public Dictionary<string, string> Dependencies { get; set; } = null!;
}