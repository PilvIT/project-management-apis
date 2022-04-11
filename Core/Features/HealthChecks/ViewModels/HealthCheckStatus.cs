using System.Net;

namespace Core.Features.HealthChecks.ViewModels;

public class HealthCheckStatus
{
    public string Code { get; set; }
    
    public HealthCheckStatus()
    {
    }

    public HealthCheckStatus(HttpStatusCode statusCode)
    {
        Code = statusCode switch
        {
            HttpStatusCode.OK => "OK",
            _ => "SERVICE_DOWN"
        };
    }
}
