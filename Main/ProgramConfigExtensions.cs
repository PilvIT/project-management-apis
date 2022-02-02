using Core;
using Main.Constants;
using Microsoft.EntityFrameworkCore;

namespace Main;

public static class ServiceExtensions
{
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
