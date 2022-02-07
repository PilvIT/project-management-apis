using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using Core;
using Core.Features.GitHubApp;
using Core.Features.GitHubApp.ApiModels;
using Core.Features.Users.Models;
using Main.ApiModels;
using Main.Injectables.Interfaces;
using Main.JsonModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Storage;
using Microsoft.IdentityModel.Tokens;

namespace Main.Api;

[ApiController]
[Route("github")]
public class AuthorizationApi : ControllerBase
{
    private readonly AppDbContext _dbContext;
    private readonly IConfiguration _conf;
    private readonly IGitHubService _github;
    private readonly UserManager<AppUser> _userManager;

    public AuthorizationApi(AppDbContext dbContext, IConfiguration conf,  IGitHubService githubService, UserManager<AppUser> userManager)
    {
        _conf = conf;
        _dbContext = dbContext;
        _github = githubService;
        _userManager = userManager;
    }
    
    [AllowAnonymous]
    [HttpPost("auth")]
    public GitHubAuthorizationUrl GetAuthorizationUrl(AuthorizationRequest requestData)
    {
        return _github.Authorization.GetUrl(requestData.RedirectUri);
    }

    [AllowAnonymous]
    [HttpPost("exchange-token")]
    public async Task<AuthorizationTokenResponse> ExchangeToken(AuthorizationTokenRequest requestData)
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
            user = await CreateUserAsync(gitHubUser);
        }

        var response = new AuthorizationTokenResponse
        {
            Token = GenerateJwtToken(user, tokens)
        };

        return response;
    }

    // TODO: Move below to own class
    
    private async Task<AppUser> CreateUserAsync(GitHubUser gitHubUser)
    {
        await using IDbContextTransaction transaction = await _dbContext.Database.BeginTransactionAsync();

        var user = new AppUser
        {
            UserName = Guid.NewGuid().ToString()
        };

        IdentityResult result = await _userManager.CreateAsync(user);
        if (result.Succeeded)
        {
            var profile = new Profile
            {
                AppUserId = user.Id,
                GitHubId = gitHubUser.Id
            };
            user.Profile = profile;
            _dbContext.Profiles.Add(profile);

            await _dbContext.SaveChangesAsync();
            transaction.Commit();

            return user;
        }
        
        throw new ArgumentException("User already exists", nameof(gitHubUser));
    }

    private string GenerateJwtToken(AppUser user, GitHubTokens tokens)
    {
        var claims = new List<Claim>
        {
            new Claim(ClaimTypes.Name, user.Id.ToString()),
            new Claim("gh-access-token", tokens.AccessToken),
            new Claim("gh-refresh-token", tokens.RefreshToken)
        };
        
        DateTime expiresAt = DateTime.UtcNow.AddHours(8);
        var signingCredentials = new SigningCredentials(_conf.GetJwtPrivateKey(), SecurityAlgorithms.RsaSha256);
        var token = new JwtSecurityToken(
            issuer: _conf.GetJwtIssuer(),
            audience: _conf.GetJwtAudience(),
            claims,
            expires: expiresAt,
            signingCredentials: signingCredentials
        );
        
        return new JwtSecurityTokenHandler().WriteToken(token);
    }
}
