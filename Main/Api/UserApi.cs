using Core;
using Core.Features.Users.Models;
using Core.Features.Users.ViewModels;
using Main.Injectables.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace Main.Api;

[Route("user")]
public class UserApi : ApiBase
{
    private readonly AppDbContext _dbContext;
    
    public UserApi(AppDbContext dbContext, IAuth auth) : base(auth)
    {
        _dbContext = dbContext;
    }
    
    [HttpGet]
    public UserDetail GetUser()
    {
        _dbContext.Entry(User).Reference(u => u.Profile).Load();
        return new UserDetail
        {
            Id = User.Id,
            Name = "Luke Skywalker"
        };
    }
}
