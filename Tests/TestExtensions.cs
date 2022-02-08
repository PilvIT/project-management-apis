using System.Net.Http;
using System.Threading.Tasks;
using Xunit.Abstractions;

namespace Tests;

public static class TestExtensions
{
    public static async Task PrintAsync(this ITestOutputHelper output, HttpResponseMessage response)
    {
        output.WriteLine(response.ToString());
        output.WriteLine(await response.Content.ReadAsStringAsync());
    }
}
