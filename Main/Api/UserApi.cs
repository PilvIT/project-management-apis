using System.Diagnostics;
using Core;
using Core.Features.Users.ViewModels;
using Core.Models;
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
        _dbContext.Entry(User).Reference(u => u.Profile).Load();
    }
    
    [HttpGet]
    public UserDetail GetUser()
    {
        return new UserDetail(User);
    }

    [HttpPut]
    public UserDetail UpdateUser(UserUpdateRequest request)
    {
        Profile profile = User.Profile!;
        profile.DisplayName = request.Name;
        profile.Description = request.Description;
        _dbContext.SaveChangesAsync();

        return new UserDetail(User);
    }
}
