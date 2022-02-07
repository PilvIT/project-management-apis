namespace Main.Middlewares
{
    /// <summary>
    /// Logs requests and durations, not needed in Production.
    /// </summary>
    public class RequestLogMiddleware
    {
        private readonly RequestDelegate _next;
        private readonly ILogger _logger;

        public RequestLogMiddleware(RequestDelegate next, ILogger<RequestLogMiddleware> logger)
        {
            _next = next;
            _logger = logger;
        }

        public async Task InvokeAsync(HttpContext httpContext)
        {
            DateTime startTime = DateTime.UtcNow;
            try
            {
                await _next(httpContext);
            }
            finally
            {
                var duration = (DateTime.UtcNow - startTime).TotalMilliseconds;

                _logger.LogInformation(
                    "{method} {url}{query} {statusCode} {duration}ms",
                    httpContext.Request.Method,
                    httpContext.Request.Path.Value,
                    httpContext.Request.QueryString.Value,
                    httpContext.Response.StatusCode,
                    duration);
            }
        }
    }
}
