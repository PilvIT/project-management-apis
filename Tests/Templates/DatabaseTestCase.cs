using System.Threading.Tasks;
using Core;
using Core.Features.GitHub.ViewModels;
using Core.Features.Users;
using Core.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.DependencyInjection;
using Xunit;

namespace Tests.Templates;

[Collection("DatabaseConnected")]
public class DatabaseTestCase : BaseTest
{
    private static object _idSequence = (long) 0;
    protected readonly AppDbContext DbContext;

    protected DatabaseTestCase()
    {
        DbContext = Setup.GetDbContext();
    }
    
    protected async Task<AppUser> CreateUserAsync()
    {
        using IServiceScope scope = Setup.WebApplicationFactory.Services.CreateScope();
        var userManager = scope.ServiceProvider.GetService<UserManager<AppUser>>()!;

        var userCreateService = new UserCreateService(dbContext: DbContext, userManager: userManager);
        var gitHubId = GetSequentialId();
        return await userCreateService.CreateAsync(new GitHubUserDetail
        {
            Id = gitHubId,
            Name = "John Doe",
            Url = "https://localhost:8000/john"
        });
    }

    protected static long GetSequentialId()
    {
        long id = 0;
        lock (_idSequence)
        {
            id = (long) _idSequence;
            _idSequence = id + 1;
        }

        return id;
    }
}
