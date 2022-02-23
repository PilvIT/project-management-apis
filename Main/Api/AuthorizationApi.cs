using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using Core;
using Core.Features.GitHub.Interfaces;
using Core.Features.GitHub.ViewModels;
using Core.Features.Users;
using Core.Models;
using Main.Injectables.Interfaces;
using Main.ViewModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
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
    public GitHubOAuthInitResponse GetAuthorizationUrl(AuthInitRequest requestData)
    {
        return _github.OAuthClient.GetUrl(requestData.RedirectUri);
    }

    [AllowAnonymous]
    [HttpPost("exchange-token")]
    public async Task<AuthTokenExchangeResponse> ExchangeToken(AuthTokenExchangeRequest requestData)
    {
        GitHubTokenDetail tokenDetail = await _github.OAuthClient.ExchangeTokenAsync(
            code: requestData.Code,
            redirectUri: requestData.RedirectUri,
            state: requestData.State);

        IGitHubUserApiClient userApi = _github.GetUserApiClient(accessToken: tokenDetail.AccessToken);
        GitHubUserDetail gitHubUserDetail = await userApi.GetUserAsync();
        
        AppUser? user = _dbContext.Users
            .Include(user => user.Profile)
            .SingleOrDefault(user => user.Profile != null && user.Profile.GitHubId == gitHubUserDetail.Id);

        if (user == null)
        {
            var userCreateService = new UserCreateService(_dbContext, _userManager);
            user = await userCreateService.CreateAsync(gitHubUserDetail.Id);
        }

        var response = new AuthTokenExchangeResponse
        {
            UserId = user.Id,
            Token = GenerateJwtToken(user, tokenDetail)
        };

        return response;
    }

    private string GenerateJwtToken(AppUser user, GitHubTokenDetail tokenDetail)
    {
        var claims = new List<Claim>
        {
            new Claim(ClaimTypes.Name, user.Id.ToString()),
            new Claim("gh-access-token", tokenDetail.AccessToken),
            new Claim("gh-refresh-token", tokenDetail.RefreshToken)
        };
        
        DateTime expiresAt = DateTime.UtcNow.AddHours(8);
        var signingCredentials = new SigningCredentials(_conf.GetJwtPrivateKey(), SecurityAlgorithms.RsaSha256);
        var token = new JwtSecurityToken(
            issuer: _conf.GetJwtIssuer(),
            audience: _conf.GetJwtAudience(),
            claims: claims,
            expires: expiresAt,
            signingCredentials: signingCredentials
        );
        
        return new JwtSecurityTokenHandler().WriteToken(token);
    }
}
