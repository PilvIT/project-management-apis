using Core.Features.GitHubApp;
using Core.Features.GitHubApp.JsonModels;
using Main.JsonModels;
using Microsoft.AspNetCore.Mvc;

namespace Main.Api;

[Route("github")]
public class AuthorizationApi : ControllerBase
{
    private readonly GitHubAuthorization _github;
    
    public AuthorizationApi(IConfiguration conf)
    {
        _github = new GitHubAuthorization(conf.GetGitHubClientId(), conf.GetGitHubClientSecret());
    }
    
    [HttpPost("auth")]
    public GitHubAuthorizationUrl GetAuthorizationUrl(AuthorizationRequest requestData)
    {
        return _github.GetAuthorizationUrl(requestData.RedirectUri);
    }

    [HttpPost("exchange-token")]
    public async Task ExchangeToken(AuthorizationTokenRequest requestData)
    {
        GitHubTokenExchangeResponse tokens = await _github.ExchangeTokenAsync(
            code: requestData.Code,
            redirectUri: requestData.RedirectUri,
            state: requestData.State);
        
        // TODO: GET INFO
        
    }
}
