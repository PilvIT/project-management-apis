using System;
using System.Collections.Generic;
using System.Net;
using System.Net.Http;
using System.Net.Http.Json;
using System.Threading.Tasks;
using Core.Extensions;
using Core.Features.HealthChecks.ViewModels;
using Core.Features.Projects;
using Core.Features.Projects.ViewModels;
using Core.Models;
using Main.Api;
using Tests.Templates;
using Xunit;
using Xunit.Abstractions;

namespace Tests.Api;

public class HealthCheckApiTest : ApiTestCase
{
    private const string BaseUrl = HealthCheckApi.BaseUrl;
    private const string TestUrl = "http://localhost:3000/ht";
    private readonly HttpClient _client;

    public HealthCheckApiTest(ITestOutputHelper o)
    {
        (_, _client) = SetupUserAsync().Result;
    }

    [Fact]
    public async Task CreateSucceeds()
    {
        var service = new ProjectService(DbContext);
        ProjectGroup group = await service.CreateProjectGroupAsync("Algorithms");
        Project project = await service.CreateProjectAsync("Comparators", group);
        var repository = new GitRepository
        {
            Name = "String Algorithms",
            Url = "http://localhost:3000/string-algorithms",
        };
        project.GitRepositories.Add(repository);        
        await DbContext.SaveChangesAsync();

        var request = new HealthCheckCreateRequest
        {
            RepositoryId = repository.Id,
            Url = TestUrl
        };
        HttpResponseMessage response = await _client.PostAsJsonAsync(BaseUrl, request);
        Assert.Equal(HttpStatusCode.OK, response.StatusCode);
        var data = await response.ReadJsonAsync<HealthCheckDetail>();
        Assert.NotNull(await DbContext.HealthChecks.FindAsync(data.Id));
        Assert.Equal(repository.Id, data.Repository);
        Assert.Equal(TestUrl, data.Url);
    }

    [Fact]
    public async Task CreateRepositoryNotFound()
    {
        var request = new HealthCheckCreateRequest
        {
            RepositoryId = Guid.NewGuid(),
            Url = TestUrl
        };
        HttpResponseMessage response = await _client.PostAsJsonAsync(BaseUrl, request);
        Assert.Equal(HttpStatusCode.BadRequest, response.StatusCode);
    }
}