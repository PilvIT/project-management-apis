using System.Net.Http.Json;

namespace Core.Extensions;

public static class HttpResponseExtensions
{
    public static async Task<T>  ReadJsonAsync<T>(this HttpResponseMessage response)
    {
        var data = await response.Content.ReadFromJsonAsync<T>();
        if (data != null)
        {
            return data;
        }

        // Warning: Do not log the response, it might include credentials or PII.
        throw new FormatException("Cannot serialize a JSON!");
    }
}
