using System;
using System.Collections.Specialized;
using System.Net;
using System.Net.Http;
using System.Net.Http.Json;
using System.Threading.Tasks;
using System.Web;
using Core.Extensions;
using Core.Features.GitHub;
using Core.Features.GitHub.Interfaces;
using Core.Features.GitHub.ViewModels;
using Core.Models;
using Main.Injectables.Interfaces;
using Main.ViewModels;
using Microsoft.Extensions.DependencyInjection;
using Moq;
using Tests.Templates;
using Xunit;

namespace Tests.Api;

public class AuthorizationApiTest : ApiTestCase
{
    private const string OAuthClientId = "clientId";
    private const string OAuthClientSecret = "clientSecret";
    private const string RedirectUri = "http://localhost:3000";

    private static Mock<IGitHubOAuthClient> _mockGitHubAuthorization = null!;
    private static Mock<IGitHubUserApiClient> _mockGitHubUserApiClient = null!;
    
    internal class MockGitHubService : IGitHubService
    {
        public IGitHubOAuthClient OAuthClient { get; }

        public MockGitHubService()
        {
            _mockGitHubAuthorization = new Mock<IGitHubOAuthClient>();
            _mockGitHubAuthorization
                .Setup(m => m.ExchangeTokenAsync(It.IsAny<string>(), It.IsAny<string>(), It.IsAny<string>()).Result)
                .Returns(new GitHubTokenDetail
                {
                    AccessToken = "access_token",
                    RefreshToken = "refresh_token"
                });
            _mockGitHubAuthorization
                .Setup(m => m.GetUrl(It.IsAny<string>()))
                .Returns(new GitHubOAuthClient(clientId: OAuthClientId, clientSecret: OAuthClientSecret).GetUrl(RedirectUri));
            
            OAuthClient = _mockGitHubAuthorization.Object;
        }
        
        public IGitHubUserApiClient GetUserApiClient(string accessToken)
        {
            _mockGitHubUserApiClient = new Mock<IGitHubUserApiClient>();
            _mockGitHubUserApiClient
                .Setup(m => m.GetUserAsync().Result)
                .Returns(new GitHubUserDetail
                {
                    Id = GetSequentialId(),
                    Name = "John Doe",
                    Url = "https://localhost:8000/john"
                });
            
            return _mockGitHubUserApiClient.Object;
        }
    }
    
    public AuthorizationApiTest()
    {
        InjectServices(services =>
        {
            services.AddTransient<IGitHubService, MockGitHubService>();
        });
    }
    
    [Fact]
    public async Task GetAuthorizationUrl()
    {
        HttpClient client = GetAnonClient();
        var requestData = new AuthInitRequest { RedirectUri = RedirectUri };
        HttpResponseMessage response = await client.PostAsJsonAsync("/github/auth", requestData);
        AssertContext(() => Assert.Equal(HttpStatusCode.OK, response.StatusCode), response);
        var responseData = await response.ReadJsonAsync<GitHubOAuthInitResponse>();
        
        var uri = new Uri(responseData.Url);
        AssertContext(() =>
        {
            Assert.Equal("https", uri.Scheme);
            Assert.Equal("github.com", uri.Host); 
        }, "The OAuth host is incorrectly configured!");

        NameValueCollection query = HttpUtility.ParseQueryString(uri.Query);
        AssertContext(() =>
        {
            Assert.Equal(OAuthClientId, query.Get("client_id"));
            Assert.Equal(requestData.RedirectUri, query.Get("redirect_uri"));
            Assert.Equal(responseData.State, query.Get("state"));
            Assert.Equal("false", query.Get("allow_signup"));
        },  $"Query string has invalid parameters, see documentation https://docs.github.com/en/developers/apps/building-github-apps/identifying-and-authorizing-users-for-github-app!");
    }
    
    [Fact]
    public async Task ExchangeTokenForNewUser()
    {
        HttpClient client = GetAnonClient();
        var requestData = new AuthTokenExchangeRequest
        {
            Code = "code-from-github",
            State = "state-as-uuid",
            RedirectUri = RedirectUri
        };
        HttpResponseMessage response = await client.PostAsJsonAsync("/github/exchange-token", requestData);
        AssertContext(() => Assert.Equal(HttpStatusCode.OK, response.StatusCode), response);
        var responseData = await response.ReadJsonAsync<AuthTokenExchangeResponse>();
        
        // Assert mock calls
        _mockGitHubAuthorization.Verify(m => m.ExchangeTokenAsync(It.IsAny<string>(), RedirectUri, It.IsAny<string>()), Times.Once);
        _mockGitHubUserApiClient.Verify(m => m.GetUserAsync().Result, Times.Once);

        // Assert initial user data
        AppUser? user = await DbContext.Users.FindAsync(responseData.UserId);
        Assert.NotNull(user);
        await DbContext.Entry(user!).Reference(u => u.Profile).LoadAsync();
    }
}
