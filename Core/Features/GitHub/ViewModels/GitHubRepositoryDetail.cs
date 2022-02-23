using System.Text.Json.Serialization;

namespace Core.Features.GitHub.ViewModels;

public class GitHubRepositoryResponse
{
    public DataResponseGraphQL Data { get; set; } = null!;
}

public class DataResponseGraphQL
{
    public RepositoryResponse Repository { get; set; } = null!;
}

public class RepositoryResponse
{
    public string Description { get; set; } = null!;
    /// <summary>
    /// One of "PUBLIC" or "PRIVATE"
    /// </summary>
    public string Visibility { get; set; } = null!;

    public VulnerabilityAlertsGraphQL VulnerabilityAlerts { get; set; } = null!;
}

public class VulnerabilityAlertsGraphQL
{
    public List<VulnerabilityNodeGraphQL> Nodes { get; set; } = null!;
    public int TotalCount { get; set; }
}

public class VulnerabilityNodeGraphQL
{
    [JsonPropertyName("number")]
    public int Id { get; set; } 
    public DateTime CreatedAt { get; set; }
    public string VulnerableManifestPath { get; set; } = null!;
    public SecurityVulnerabilityGraphQL SecurityVulnerability { get; set; } = null!;
}

public class SecurityVulnerabilityGraphQL
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

    public AdvisoryGraphQL Advisory { get; set; } = null!;

}

public class AdvisoryGraphQL
{
    public string Summary { get; set; } = null!;

    [JsonPropertyName("cvss")]
    public Dictionary<string, double> Cvss { get; set; } = null!;
    [JsonIgnore]
    public double CvssScore => Cvss["score"];
}
