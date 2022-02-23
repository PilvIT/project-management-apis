using System;
using System.Net.Http;
using System.Net.Http.Json;
using System.Threading.Tasks;
using Core.Features.Projects.ApiModels;
using Core.Features.Projects.ViewModels;
using Core.Models;
using Tests.Templates;
using Xunit;

namespace Tests.Api;

public class ProjectApiTest : ApiTestCase
{
    public ProjectApiTest()
    {
    }
    
    [Fact]
    public void GetProjects()
    {
        throw new NotImplementedException();
    }

    [Fact]
    public async Task CreateProject()
    {
        (_, HttpClient client) = await SetupUserAsync();
        var requestData = new ProjectCreateRequest
        {
            GroupName = "Example Oyj",
            Name = "Candy Shop"
        };
        HttpResponseMessage response = await client.PostAsJsonAsync("/projects", requestData);

        var responseData = (await response.Content.ReadFromJsonAsync<ProjectDetail>())!;
        Project project = (await DbContext.Projects.FindAsync(responseData.Id))!;
        await DbContext.Entry(project).Reference(p => p.Group).LoadAsync();
        
        // Assert database
        Assert.Equal(requestData.GroupName, project.Group!.Name);
        Assert.Equal(requestData.Name, project.Name);
        
        // Assert response
        Assert.Equal(project.Id, responseData.Id);
        Assert.Equal(project.Name, responseData.Name);
        Assert.Equal(project.Group.Name, responseData.Group);
    }
}
