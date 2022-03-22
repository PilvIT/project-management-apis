using System.Diagnostics;
using Core;
using Core.Features.Users.ViewModels;
using Main.Injectables.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace Main.Api;

[Route("user")]
public class UserApi : BaseApi
{
    private readonly AppDbContext _dbContext;
    
    public UserApi(AppDbContext dbContext, IAuthService authService) : base(authService)
    {
        _dbContext = dbContext;
    }
    
    [HttpGet]
    public UserDetail GetUser()
    {
        _dbContext.Entry(User).Reference(u => u.Profile).Load();
        Debug.Assert(User.Profile != null);
        
        return new UserDetail
        {
            Id = User.Id,
            Name = User.Profile.DisplayName,
            Email = User.Email,
            GitHub = User.Profile.GitHubUrl
        };
    }
}
