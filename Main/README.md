# Main Server

Define the following JSON named `secrets.json` and load it into [DotNet Secrets](https://docs.microsoft.com/en-us/aspnet/core/security/app-secrets)

```
{
    "Integrations": {
        "GitHub": {
            "ClientId": "",
            "ClientSecret": ""
        }
    }
}
```

```ps1
type .\secrets.json | dotnet user-secrets set
```