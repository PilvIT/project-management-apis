using System;
using System.Collections.Specialized;
using System.Net;
using System.Net.Http;
using System.Net.Http.Json;
using System.Threading.Tasks;
using System.Web;
using Core.Features.GitHubApp;
using Core.Features.GitHubApp.ApiModels;
using Main.ApiModels;
using Main.Injectables.Interfaces;
using Microsoft.Extensions.DependencyInjection;
using Moq;
using Tests.Templates;
using Xunit;

namespace Tests.Api;

public class AuthorizationApiTest : ApiTestCase
{
    private const string OAuthClientId = "clientId";
    private const string OAuthClientSecret = "clientSecret";
    private static Mock<GitHubAuthorization> MockGitHubAuthorization = null!;
    
    internal class MockGitHubService : IGitHubService
    {
        public GitHubAuthorization Authorization { get; }

        public MockGitHubService()
        {
            MockGitHubAuthorization = new Mock<GitHubAuthorization>(OAuthClientId, OAuthClientSecret);
            Authorization = MockGitHubAuthorization.Object;
        }
        
        public GitHubUserApiClient GetUserApiClient(string accessToken)
        {
            throw new NotImplementedException("this one?");
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
        var requestData = new AuthorizationRequest { RedirectUri = "http://localhost:3000" };
        HttpResponseMessage response = await client.PostAsJsonAsync("/github/auth", requestData);
        AssertContext(() => Assert.Equal(HttpStatusCode.OK, response.StatusCode), response);
        var responseData = await response.Content.ReadFromJsonAsync<GitHubAuthorizationUrl>();
        
        var uri = new Uri(responseData!.Url);
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
}
