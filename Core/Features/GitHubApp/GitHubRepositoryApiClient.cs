using System.IO.Compression;
using System.Net.Http.Headers;
using Core.Extensions;
using Core.Features.GitHubApp.ApiModels;
using Microsoft.Net.Http.Headers;

namespace Core.Features.GitHubApp;

public class GitHubRepositoryApiClient
{
    private const string Host = "https://api.github.com";
    private readonly HttpClient _httpClient;
    
    public GitHubRepositoryApiClient(string appName, GitHubTokens tokens)
    {
        _httpClient = new HttpClient();
        _httpClient.BaseAddress = new Uri(Host);
        _httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("token", tokens.AccessToken);
        _httpClient.DefaultRequestHeaders.Add(HeaderNames.Accept, "application/vnd.github.v3+json");
        _httpClient.DefaultRequestHeaders.Add(HeaderNames.UserAgent, appName);
    }
    
    public async Task<object> GetLatestArtifacts(string repositoryUrl, string targetDirectory)
    {
        var url = $"{repositoryUrl}/actions/artifacts".Replace("https://github.com/", $"{Host}/repos/");
        HttpResponseMessage response = await _httpClient.GetAsync(url);
        var artifactData =  await response.ReadJsonAsync<GitHubArtifactListResponse>();
        if (artifactData.Count > 0)
        {
            string path = await DownloadArtifactAsync(artifactData.Artifacts[0], targetDirectory);
        }
        return null;
    }
    
    private async Task<string> DownloadArtifactAsync(GitHubArtifactListDetail artifact, string targetDirectory)
    {
        var zipPath = $"{targetDirectory}/${artifact.NodeId}.zip";
        var unzipPath = $"{targetDirectory}/${artifact.NodeId}";
        
        HttpResponseMessage response = await _httpClient.GetAsync(artifact.DownloadUrl);
        Stream stream = await response.Content.ReadAsStreamAsync();

        await using var fs = new FileStream(zipPath, FileMode.Create, FileAccess.Write, FileShare.None);
        await stream.CopyToAsync(fs);
        fs.Close();

        ZipFile.ExtractToDirectory(zipPath, unzipPath);
        File.Delete(zipPath);
        return unzipPath;
    }
}