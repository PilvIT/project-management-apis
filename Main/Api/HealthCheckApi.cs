using Core.Features.HealthChecks;
using Core.Features.HealthChecks.ViewModels;
using Core.Models;
using Main.Injectables.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace Main.Api;

[Route(BaseUrl)]
public class HealthCheckApi : BaseApi
{
    public const string BaseUrl = "health-checks";
    private readonly IHealthCheckService _service;
    
    public HealthCheckApi(IAuthService authService, IHealthCheckService service) : base(authService)
    {
        _service = service;
    }

    [HttpPost]
    public async Task<HealthCheckDetail> CreateHealthCheck(HealthCheckCreateRequest request)
    {
        HealthCheck healthCheck = await _service.AddHealthCheckUrlAsync(request.Repository, request.Url);
        return new HealthCheckDetail(healthCheck);
    }
}
