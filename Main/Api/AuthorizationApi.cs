using Core;
using Core.Features.GitHubApp;
using Core.Features.GitHubApp.ApiModels;
using Core.Features.Users.Models;
using Main.ApiModels;
using Main.JsonModels;
using Main.Services;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace Main.Api;

[Route("github")]
public class AuthorizationApi : ControllerBase
{
    private readonly AppDbContext _dbContext;
    private readonly IGitHubService _github;
    private readonly SignInManager<AppUser> _signInManager;

    public AuthorizationApi(AppDbContext dbContext, IGitHubService githubService, SignInManager<AppUser> signInManager)
    {
        _dbContext = dbContext;
        _github = githubService;
        _signInManager = signInManager;
    }
    
    [HttpPost("auth")]
    public GitHubAuthorizationUrl GetAuthorizationUrl(AuthorizationRequest requestData)
    {
        return _github.Authorization.GetUrl(requestData.RedirectUri);
    }

    [HttpPost("exchange-token")]
    public async Task<GitHubTokens> ExchangeToken(AuthorizationTokenRequest requestData)
    {
        GitHubTokens tokens = await _github.Authorization.ExchangeTokenAsync(
            code: requestData.Code,
            redirectUri: requestData.RedirectUri,
            state: requestData.State);

        GitHubUserApiClient userApi = _github.GetUserApiClient(accessToken: tokens.AccessToken);
        GitHubUser gitHubUser = await userApi.GetUserAsync();
        
        AppUser? user = _dbContext.Users
            .Include(user => user.Profile)
            .SingleOrDefault(user => user.Profile != null && user.Profile.GitHubId == gitHubUser.Id);

        if (user == null)
        {
            // TODO: create user if not exist
        }
        

        // TODO: return identity token
        return tokens;
    }
}
