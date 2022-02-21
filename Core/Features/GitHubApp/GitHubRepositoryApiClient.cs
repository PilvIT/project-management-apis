using System.IO.Compression;
using System.Net.Http.Json;
using Core.Extensions;
using Core.Features.GitHubApp.ApiModels;
using Core.Features.Projects.Models;

namespace Core.Features.GitHubApp;

public class GitHubRepositoryApiClient : GitHubBaseApi, IGitHubRepositoryApiClient
{
    public GitHubRepositoryApiClient(string appName, GitHubTokens tokens) : base(appName, tokens)
    {
    }
    
    /// <summary>
    /// Finds the latest artifact and downloads it.
    /// </summary>
    /// <param name="repository">to find artifacts</param>
    /// <param name="targetDirectory">directory </param>
    /// <returns>path to unzipped artifact or null if no artifact exists</returns>
    public async Task<string?> GetLatestArtifacts(GitRepository repository, string targetDirectory)
    {
        var url = $"{repository.Url}/actions/artifacts".Replace("https://github.com/", $"{Host}/repos/");
        HttpResponseMessage response = await CreateHttpClient().GetAsync(url);
        var data =  await response.ReadJsonAsync<GitHubArtifactListResponse>();
        if (data.Count > 0 && !data.Artifacts[0].Expired)
        {
             return await DownloadArtifactAsync(data.Artifacts[0], targetDirectory);
        }

        return null;
    }

    /// <summary>
    /// Reads the repository detail and dependabot alerts from GraphQL endpoint.
    /// </summary>
    public async Task<GitHubRepositoryResponse> GetRepositoryDetailAsync(GitRepository repository)
    {
        var request = new GitHubRepositoryRequest(repository.Url);
        HttpResponseMessage response = await CreateHttpClient().PostAsJsonAsync("/graphql", request);
        // TODO: Error handling
        return await response.ReadJsonAsync<GitHubRepositoryResponse>();
    } 
    
    private async Task<string> DownloadArtifactAsync(GitHubArtifactListDetail artifact, string targetDirectory)
    {
        var zipPath = $"{targetDirectory}/${artifact.NodeId}.zip";
        var unzipPath = $"{targetDirectory}/${artifact.NodeId}";

        // Prevent unnecessary downloads.
        if (Directory.Exists(unzipPath))
        {
            return unzipPath;
        }
        
        HttpResponseMessage response = await CreateHttpClient().GetAsync(artifact.DownloadUrl);
        Stream stream = await response.Content.ReadAsStreamAsync();

        await using var fs = new FileStream(zipPath, FileMode.Create, FileAccess.Write, FileShare.None);
        await stream.CopyToAsync(fs);
        fs.Close();

        ZipFile.ExtractToDirectory(zipPath, unzipPath);
        File.Delete(zipPath);
        return unzipPath;
    }
}
