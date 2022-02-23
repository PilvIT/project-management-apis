using System.Security.Claims;
using Core;
using Core.Features.GitHub.ViewModels;
using Core.Models;
using Main.Exceptions;
using Main.Injectables.Interfaces;

namespace Main.Injectables;

/// <summary>
/// Handles the JWT token reads for API endpoints.
/// </summary>
public class AuthServiceService : IAuthService
{
    public AppUser User { get; }
    public GitHubTokenDetail GitHubTokenDetail => LoadGitHubTokens();

    private readonly AppDbContext _dbContext;
    private readonly HttpContext _httpContext;
    private readonly ILogger<AuthServiceService> _logger;

    public AuthServiceService(AppDbContext dbContext, IHttpContextAccessor httpContextAccessor, ILogger<AuthServiceService> logger)
    {
        _dbContext = dbContext;
        _httpContext = httpContextAccessor.HttpContext!;
        _logger = logger;
        User = LoadUser();
    }

    private GitHubTokenDetail LoadGitHubTokens()
    {
        Claim accessToken = _httpContext.User.Claims.Single(claim => claim.Type == "gh-access-token");
        Claim refreshToken = _httpContext.User.Claims.Single(claim => claim.Type == "gh-refresh-token");

        return new GitHubTokenDetail()
        {
            AccessToken = accessToken.Value,
            RefreshToken = refreshToken.Value
        };
    }
    
    private AppUser LoadUser()
    {
        // If you have problems with id, the issue is likely with JWT generation.
        Guid userId = Guid.Parse(_httpContext.User.Identity!.Name!);
        try
        {
            AppUser? user = _dbContext.Users.Find(userId);
            if (user != null)
            {
                return user;
            }
        }
        catch (Exception)
        {
            _logger.LogCritical("The endpoint is not secured, ensure that [Authorize] attribute is set");
        }
        
        throw new UnauthorizedApiException();
    }
}
