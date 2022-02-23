using System.Security.Cryptography;

namespace Main;

public static class DevelopmentSetup
{
    public static void Setup(IConfiguration conf)
    {
        GenerateJwtKeys(conf);
    } 
    
    private static void GenerateJwtKeys(IConfiguration conf)
    {
        var keyDirectory = conf.GetCertKeyDirectory();
        var publicKeyPath = conf.GetJwtPublicKeyPath();
        var privateKeyPath = conf.GetJwtPrivateKeyPath();
        
        if (File.Exists(publicKeyPath) && File.Exists(privateKeyPath))
        {
            return;
        }
        
        var rsa = RSA.Create(4096);
        // 2048 bits is good only until 2030, https://security.stackexchange.com/a/65180
        // Using the highest one better demonstrates authentication speed
        var privateKey = Convert.ToBase64String(rsa.ExportPkcs8PrivateKey());
        var publicKey = Convert.ToBase64String(rsa.ExportSubjectPublicKeyInfo());

        Directory.CreateDirectory(keyDirectory);
        File.WriteAllText(privateKeyPath, $"-----BEGIN PRIVATE KEY-----\n{privateKey}\n-----END PRIVATE KEY-----");
        File.WriteAllText(publicKeyPath, $"-----BEGIN PUBLIC KEY-----\n{publicKey}\n-----END PUBLIC KEY-----");
    }
}