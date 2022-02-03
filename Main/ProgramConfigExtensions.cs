using System.Text.Json;
using System.Text.Json.Serialization;
using Core;
using Main.Constants;
using Microsoft.EntityFrameworkCore;

namespace Main;

public static class ServiceExtensions
{
    public static void AddApi(this IServiceCollection services)
    {
        services.AddControllers().AddJsonOptions(options =>
        {
            options.JsonSerializerOptions.DictionaryKeyPolicy = JsonNamingPolicy.CamelCase;
            options.JsonSerializerOptions.PropertyNamingPolicy = JsonNamingPolicy.CamelCase;
            options.JsonSerializerOptions.Converters.Add(new JsonStringEnumConverter(JsonNamingPolicy.CamelCase));
        });
    }
    
    public static void AddDatabases(this IServiceCollection services, IConfiguration configuration)
    {
        var connectionString = configuration.GetConnectionString(ConfKeys.MainDatabase);
        services.AddDbContext<AppDbContext>(options =>
        {
            options.UseNpgsql(connectionString, npgsqlOptions =>
            {
                // Need to state explicitly since models are in Core application.
                npgsqlOptions.MigrationsAssembly("Main");
            });
        });
    }
}
