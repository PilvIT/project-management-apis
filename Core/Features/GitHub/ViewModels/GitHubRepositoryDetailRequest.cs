namespace Core.Features.GitHub.ViewModels;

public class GitHubRepositoryDetailRequest
{
    public string Query { get; } = @"
        query($name: String!, $owner: String!) {
            repository(name: $name, owner: $owner) {
                description
                visibility
                vulnerabilityAlerts(first: 10, states: OPEN) {
                    totalCount
                    nodes {
                        createdAt
                        number
                        vulnerableManifestPath
                        securityVulnerability {
                            vulnerableVersionRange
                            firstPatchedVersion {
                                identifier
                            }
                            package {
                                name
                                ecosystem
                            }
                            advisory {
                                summary
                                cvss {
                                    score
                                }
                            }
                        }
                    }
                }
            }
        }";
    public Dictionary<string, string> Variables { get; }

    public GitHubRepositoryDetailRequest(string repositoryUrl)
    {
        var parts = repositoryUrl.Split("/");
        Variables = new Dictionary<string, string>
        {
            { "name", parts[^1] },
            { "owner", parts[^2] }
        };
    }
}
