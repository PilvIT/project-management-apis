namespace Main;

public static class ConfigurationExtensions
{
    public static string GetClientHost(this IConfiguration conf) => conf["App:ClientHost"];
    public static string GetGitHubClientId(this IConfiguration conf) => conf["Integrations:GitHub:ClientId"];
    public static string GetGitHubClientSecret(this IConfiguration conf) => conf["Integrations:GitHub:ClientSecret"];
}