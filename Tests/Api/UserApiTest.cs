using System.Linq;
using System.Net.Http;
using System.Net.Http.Json;
using System.Threading.Tasks;
using Core.Extensions;
using Core.Features.Users.ViewModels;
using Core.Models;
using Tests.Templates;
using Xunit;

namespace Tests.Api;

public class UserApiTest : ApiTestCase
{
    [Fact]
    public async Task GetUser()
    {
        (AppUser user, HttpClient client) = await SetupUserAsync();
        var responseData = await client.GetFromJsonAsync<UserDetail>("/user");
        Assert.Equal( responseData!.Id, user.Id);
        Assert.Equal(responseData.Name, user.Profile!.DisplayName);
        Assert.Equal(responseData.Description, user.Profile!.Description);
        Assert.Equal(responseData.Email, user.Email);
        Assert.Equal(responseData.GitHub, user.Profile.GitHubUrl);
        // Email is retrieved from GitHub so it must be valid.
        Assert.True(user.EmailConfirmed);
    }

    [Fact]
    public async Task UpdateUser()
    {
        (_, HttpClient client) = await SetupUserAsync();
        var requestData = new UserUpdateRequest
        {
            Name = "Jane Doe",
            Description = "I am JS engineer."
        };

        HttpResponseMessage responseData = await client.PutAsJsonAsync("/user", requestData);
        var userData = await responseData.ReadJsonAsync<UserDetail>();
        
        Assert.Equal(userData.Name, requestData.Name);
        Assert.Equal(userData.Description, requestData.Description);
        Assert.True(DbContext.Profiles.Any(p => p.DisplayName == requestData.Name && p.Description == requestData.Description));
    }
}
