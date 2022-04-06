using System.Linq;
using System.Net;
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
    private readonly AppUser _user;
    private readonly HttpClient _client;
    
    public UserApiTest()
    {
        (_user, _client) = SetupUserAsync().Result;
    }
    
    [Fact]
    public async Task GetUser()
    {
        var response = await _client.GetFromJsonAsync<UserDetail>("/user");
        Assert.Equal( response!.Id, _user.Id);
        Assert.Equal(response.Name, _user.Profile!.DisplayName);
        Assert.Equal(response.Description, _user.Profile!.Description);
        Assert.Equal(response.Email, _user.Email);
        Assert.Equal(response.GitHub, _user.Profile.GitHubUrl);
        // Email is retrieved from GitHub so it must be valid.
        Assert.True(_user.EmailConfirmed);
    }

    [Fact]
    public async Task UpdateUser()
    {
        var request = new UserUpdateRequest { Name = "Jane Doe", Description = "I am JS engineer." };
        HttpResponseMessage response = await _client.PutAsJsonAsync("/user", request);
        var data = await response.ReadJsonAsync<UserDetail>();
        
        Assert.Equal(data.Name, request.Name);
        Assert.Equal(data.Description, request.Description);
        Assert.True(DbContext.Profiles.Any(p => p.DisplayName == request.Name && p.Description == request.Description));
    }

    [Fact]
    public async Task UpdateUserInvalidData()
    {
        var request = new UserUpdateRequest { Name = "", Description = "" };
        HttpResponseMessage response = await _client.PutAsJsonAsync("/user", request);
        Assert.Equal(HttpStatusCode.BadRequest, response.StatusCode);
    }
}
