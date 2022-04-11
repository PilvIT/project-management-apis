namespace Core.Extensions;

public static class HttpClientFactoryExtensions
{
    public static HttpClient CreateRetryClient(this IHttpClientFactory httpClientFactory)
    {
        return httpClientFactory.CreateClient("Retryable");
    }
}
