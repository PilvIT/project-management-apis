using System.Threading.Tasks;
using Core;
using Core.Features.Users;
using Core.Features.Users.Models;
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
        long gitHubId = 0;
        lock (_idSequence)
        {
            gitHubId = (long) _idSequence;
            _idSequence = gitHubId + 1;
        }
        return await userCreateService.CreateAsync(gitHubId);
    }
}
