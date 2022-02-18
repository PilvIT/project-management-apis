using System.ComponentModel;
using System.Text.Json.Serialization;

namespace Core.Features.GitHubApp.ApiModels;

public class GitHubRepositoryResponse
{
    public DataResponse Data { get; set; } = null!;
}

[EditorBrowsable(EditorBrowsableState.Never)]
public class DataResponse
{
    public RepositoryResponse Repository { get; set; } = null!;
}

[EditorBrowsable(EditorBrowsableState.Never)]
public class RepositoryResponse
{
    public string Description { get; set; } = null!;
    /// <summary>
    /// One of "PUBLIC" or "PRIVATE"
    /// </summary>
    public string Visibility { get; set; } = null!;

    public VulnerabilityAlerts VulnerabilityAlerts { get; set; } = null!;
}

[EditorBrowsable(EditorBrowsableState.Never)]
public class VulnerabilityAlerts
{
    [EditorBrowsable(EditorBrowsableState.Never)]
    public class Vulnerability
    {
        [JsonPropertyName("number")]
        public int Id { get; set; } 
        public DateTime CreatedAt { get; set; }
        public string VulnerableManifestPath { get; set; } = null!;
        public SecurityVulnerability SecurityVulnerability { get; set; } = null!;
    }

    public List<Vulnerability> Nodes { get; set; } = null!;
    public int TotalCount { get; set; }
}

[EditorBrowsable(EditorBrowsableState.Never)]
public class SecurityVulnerability
{
    [JsonPropertyName("firstPatchedVersion")]
    public Dictionary<string, string> JsonFirstPatchedVersion { get; set; } = null!;

    public string VulnerableVersionRange { get; set; } = null!;
    
    [JsonIgnore]
    public string FirstPatchedVersion => JsonFirstPatchedVersion["identifier"];

    [JsonPropertyName("package")]
    public Dictionary<string, string> JsonPackage { get; set; } = null!;
    [JsonIgnore]
    public string PackageName => JsonPackage["name"];
    [JsonIgnore]
    public string Ecosystem => JsonPackage["ecosystem"];

    public Advisory Advisory { get; set; } = null!;

}

[EditorBrowsable(EditorBrowsableState.Never)]
public class Advisory
{
    public string Summary { get; set; } = null!;

    [JsonPropertyName("cvss")]
    public Dictionary<string, double> Cvss { get; set; } = null!;
    [JsonIgnore]
    public double CvssScore => Cvss["score"];
}
