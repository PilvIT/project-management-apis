using Main;
using Main.Injectables;
using Main.Injectables.Interfaces;
using Main.Middlewares;
using Microsoft.AspNetCore.HttpLogging;

WebApplicationBuilder builder = WebApplication.CreateBuilder(args);
builder.Services.AddApi();
builder.Services.AddCorsPolicies();
builder.Services.AddDatabases(builder.Configuration);
builder.Services.AddJwtAuthentication(builder.Configuration);

// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

builder.Services.AddHttpLogging(options =>
{
    options.LoggingFields = HttpLoggingFields.All;
});

// Dependency injections
builder.Services.AddTransient<IGitHubService, GitHubService>();
builder.Services.AddTransient<IAuthService, AuthServiceService>();

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

app.Run();

namespace Main
{
    public partial class Program { }
}
