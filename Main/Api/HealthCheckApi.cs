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
        HealthCheck healthCheck = await _service.AddHealthCheckUrlAsync(request.Repository!, request.Url);
        return new HealthCheckDetail(healthCheck);
    }

    [HttpGet("id:guid")]
    public async Task<ActionResult<HealthCheckDetail>> RetrieveHealthCheck(Guid id, [FromQuery] bool poll)
    {
        HealthCheck? instance = await _service.RetrieveAsync(id);
        if (instance == null)
        {
            return new NotFoundResult();
        }

        if (!poll)
        {
            return new HealthCheckDetail(instance);
        }
        
        HealthCheckStatus status = await _service.CheckHealthAsync(instance);
        return new HealthCheckDetail(instance, status);

    }
    
    [HttpDelete("id:guid")]
    public async Task<ActionResult> DeleteHealthCheck(Guid id)
    {
        await _service.DeleteHealthCheckAsync(id);
        return new OkObjectResult(new { });
    }
}
