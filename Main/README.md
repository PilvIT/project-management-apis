# Main Server

Define the following JSON named `secrets.json` in the root directory and load it into [DotNet Secrets](https://docs.microsoft.com/en-us/aspnet/core/security/app-secrets)

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

You can obtain the GitHub credentials by creating an [GitHubApp](https://docs.github.com/en/developers/apps/getting-started-with-apps/about-apps).

```ps1
type .\secrets.json | dotnet user-secrets set
```

## Development

The database can be conveniently set up using _docker-compose_.

```bash
# Console 1
docker-compose up
```

Wait for the containers to start, then seed the database

```bash
# Console 2
dotnet run --project=DatabaseSeeder
```

And finally start the development server

```bash
# Console 2
dotnet watch --project=main --launch-profile Main
```
