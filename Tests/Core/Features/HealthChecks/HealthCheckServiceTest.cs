using System;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using Core.Features.HealthChecks;
using Core.Features.HealthChecks.ViewModels;
using Core.Models;
using Moq;
using Tests.Mocks;
using Tests.Templates;
using Xunit;

namespace Tests.Core.Features.HealthChecks;

public class HealthCheckServiceTest : DatabaseTestCase
{
    [Fact]
    public async Task CheckHealthAsyncOk()
    {
        var mockHttp = new HttpTestMock();
        mockHttp.MockOnce(HttpStatusCode.OK, "{}");
        
        var instance = new HealthCheck { Url = "http://localhost:5000/health-check" };
        var service = new HealthCheckService(DbContext, mockHttp.HttpClientFactory);
        HealthCheckStatus status = await service.CheckHealthAsync(instance);
        Assert.Equal("OK", status.Code);
        
        mockHttp.VerifyRequest(request 
            => request.Method == HttpMethod.Get 
               && request.RequestUri == new Uri(instance.Url), Times.Once());
    }
    
    [Fact]
    public async Task CheckHealthAsyncFailed()
    {
        var mockHttp = new HttpTestMock();
        mockHttp.MockOnce(HttpStatusCode.InternalServerError, "{}");
        var instance = new HealthCheck { Url = "http://localhost:5000/health-check" };
        var service = new HealthCheckService(DbContext, mockHttp.HttpClientFactory);
        HealthCheckStatus status = await service.CheckHealthAsync(instance);
        Assert.Equal("SERVICE_DOWN", status.Code);
    }
}
