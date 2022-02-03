using System.Security.Cryptography;
using Microsoft.IdentityModel.Tokens;

namespace Main;

public static class ConfigurationExtensions
{
    public static string GetClientHost(this IConfiguration conf) => conf["App:ClientHost"];
    public static string GetGitHubClientId(this IConfiguration conf) => conf["Integrations:GitHub:ClientId"];
    public static string GetGitHubClientSecret(this IConfiguration conf) => conf["Integrations:GitHub:ClientSecret"];

    public static string GetCertKeyDirectory(this IConfiguration conf) => conf["CertKeyDirectory"];
    public static string GetJwtPublicKeyPath(this IConfiguration conf) => $"{conf.GetCertKeyDirectory()}/jwt.public.pem";
    public static RsaSecurityKey GetJwtPublicKey(this IConfiguration conf) => ReadRsaKey(conf.GetJwtPublicKeyPath());
    public static string GetJwtPrivateKeyPath(this IConfiguration conf) => $"{conf.GetCertKeyDirectory()}/jwt.private.pem";
    public static RsaSecurityKey GetJwtPrivateKey(this IConfiguration conf) => ReadRsaKey(conf.GetJwtPrivateKeyPath());
    public static string GetJwtAudience(this IConfiguration conf) => conf["Authentication:JwtAudience"];
    public static string GetJwtIssuer(this IConfiguration conf) => conf["Authentication:JwtIssuer"];
    
    private static RsaSecurityKey ReadRsaKey(string path)
    {
        string fileContent = File.ReadAllText(path);
        var rsaKey = RSA.Create();
        rsaKey.ImportFromPem(fileContent);
        return new RsaSecurityKey(rsaKey);
    }
}
