using Core.Features.HealthChecks;
using Core.Features.Projects;
using Main;
using Main.Injectables;
using Main.Injectables.Interfaces;
using Main.Middlewares;
using Microsoft.AspNetCore.HttpLogging;

WebApplicationBuilder builder = WebApplication.CreateBuilder(args);
builder.Services.AddApi();
builder.Services.AddCorsPolicies();
builder.Services.AddDatabases(builder.Configuration);
builder.Services.AddHealthChecks();
builder.Services.AddJwtAuthentication(builder.Configuration);

// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

builder.Services.AddHttpLogging(options =>
{
    options.LoggingFields = HttpLoggingFields.All;
});


// Dependency injections
builder.Services.AddRetryableHttpClient();
builder.Services.AddTransient<IAuthService, AuthServiceService>();
builder.Services.AddTransient<IHealthCheckService, HealthCheckService>();
builder.Services.AddTransient<IGitHubService, GitHubService>();
builder.Services.AddTransient<IProjectService, ProjectService>();

WebApplication app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    DevelopmentSetup.Setup(app.Configuration);
    app.UseSwagger();
    app.UseSwaggerUI();
    app.UseCors("localhost");
    app.UseMiddleware<RequestLogMiddleware>();
}
else
{
    // Production configuration
    app.UseCors();
    app.UseHttpsRedirection();
}

app.UseAuthentication();
app.UseAuthorization();
app.MapControllers().RequireAuthorization();
app.MapHealthChecks("/health-check");

app.Run();

namespace Main
{
    public partial class Program { }
}
