using Core;
using Main;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;

namespace Tests;

public static class Setup
{
    public static readonly IConfiguration Configuration = ReadConfigurationFiles();
    public static readonly DbContextOptions<AppDbContext> DbContextOptions = InitDatabase();
    public static readonly WebApplicationFactory<Program> WebApplicationFactory = InitWebApplicationFactory();

    public static AppDbContext GetDbContext() => new AppDbContext(DbContextOptions);

    private static DbContextOptions<AppDbContext> InitDatabase()
    {
        var dbContextBuilder = new DbContextOptionsBuilder<AppDbContext>();
        dbContextBuilder.UseNpgsql(Configuration.GetConnectionString("PostgreSQL"));

        var db = new AppDbContext(dbContextBuilder.Options);
        db.Database.EnsureDeleted();
        db.Database.EnsureCreated();

        return dbContextBuilder.Options;
    }

    private static WebApplicationFactory<Program> InitWebApplicationFactory()
    {
        return new WebApplicationFactory<Program>()
            .WithWebHostBuilder(builder =>
            {
                builder.UseConfiguration(Setup.Configuration);
                builder.UseEnvironment("Development");
            });
    }

    private static IConfiguration ReadConfigurationFiles()
    {
        var configurationBuilder = new ConfigurationBuilder();
        configurationBuilder.AddJsonFile("appsettings.json");
        configurationBuilder.AddJsonFile("appsettings.Development.json");
        configurationBuilder.AddJsonFile("appsettings.Testing.json");

        return configurationBuilder.Build();
    }
}