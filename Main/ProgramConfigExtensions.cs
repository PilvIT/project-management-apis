using System.Text.Json;
using System.Text.Json.Serialization;
using Core;
using Core.Models;
using Main.Exceptions;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using Microsoft.Net.Http.Headers;
using Polly;

namespace Main;

public static class ServiceExtensions
{
    public static void AddApi(this IServiceCollection services)
    {
        services
            .AddControllers(options =>
            {
                options.Filters.Add(new HttpResponseExceptionFilter());
            })
            .AddJsonOptions(options =>
            {
                options.JsonSerializerOptions.DictionaryKeyPolicy = JsonNamingPolicy.CamelCase;
                options.JsonSerializerOptions.PropertyNamingPolicy = JsonNamingPolicy.CamelCase;
                options.JsonSerializerOptions.Converters.Add(new JsonStringEnumConverter(JsonNamingPolicy.CamelCase));
                options.JsonSerializerOptions.ReferenceHandler = ReferenceHandler.IgnoreCycles;
            });
    }

    public static void AddCorsPolicies(this IServiceCollection services)
    {
        services.AddCors(options =>
        {
            options.AddDefaultPolicy(policy =>
            {
                policy.WithHeaders(HeaderNames.Authorization, HeaderNames.ContentType);
                policy.WithExposedHeaders(HeaderNames.ContentType);
                policy.WithMethods("GET", "POST", "PATCH", "PUT", "DELETE", "OPTIONS");
            });
            options.AddPolicy("localhost", policy =>
            {
                policy.AllowAnyOrigin();
                policy.WithHeaders(HeaderNames.Authorization, HeaderNames.ContentType);
                policy.WithExposedHeaders(HeaderNames.ContentType);
                policy.WithMethods("GET", "POST", "PATCH", "PUT", "DELETE", "OPTIONS");
            });
        });
    }

    public static void AddJwtAuthentication(this IServiceCollection services, IConfiguration conf)
    {
        services
            .AddIdentity<AppUser, AppRole>()
            .AddDefaultTokenProviders()
            .AddUserManager<UserManager<AppUser>>()
            .AddSignInManager<SignInManager<AppUser>>()
            .AddEntityFrameworkStores<AppDbContext>();

        services
            .AddAuthentication(options =>
            {
                options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
                options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
            })
            .AddJwtBearer(options =>
            {
                options.TokenValidationParameters = new TokenValidationParameters
                {
                    ValidateAudience = true, // audience = client domain
                    ValidAudience = conf.GetJwtAudience(),
                    ValidateIssuer = true, // issuer = server domain
                    ValidateIssuerSigningKey = true,
                    ValidIssuer = conf.GetJwtIssuer(),
                    ValidateLifetime = true,
                    IssuerSigningKey = conf.GetJwtPublicKey()
                };
            });
    }

    public static void AddDatabases(this IServiceCollection services, IConfiguration configuration)
    {
        string? connectionString = configuration.GetConnectionString("PostgreSQL");
        services.AddDbContext<AppDbContext>(options =>
        {
            options.UseNpgsql(connectionString, npgsqlOptions =>
            {
                // Need to state explicitly since models are in Core application.
                npgsqlOptions.MigrationsAssembly("Main");
            });
        });
    }

    public static void AddRetryableHttpClient(this IServiceCollection services)
    {
        // https://docs.microsoft.com/en-us/aspnet/core/fundamentals/http-requests?view=aspnetcore-6.0#use-polly-based-handlers
        services
            .AddHttpClient("Retryable")
            .AddTransientHttpErrorPolicy(
                options => 
                    options.WaitAndRetryAsync(3, count => TimeSpan.FromMilliseconds(count * 500)));
    }
}
