using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Security.Claims;
using System.Threading.Tasks;
using Core.Models;
using Main;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.IdentityModel.Tokens;
using Microsoft.Net.Http.Headers;

namespace Tests.Templates;

public class ApiTestCase : DatabaseTestCase
{
    private readonly IConfiguration _conf = Setup.Configuration;
    private WebApplicationFactory<Program> _app = Setup.WebApplicationFactory;
    
    protected void InjectServices(Action<IServiceCollection> injector)
    {
        _app = Setup.WebApplicationFactory.WithWebHostBuilder(builder => builder.ConfigureServices(injector.Invoke));
    }
    
    protected HttpClient GetAnonClient() => _app.CreateClient();

    protected async Task<(AppUser, HttpClient)> SetupUserAsync()
    {
        AppUser user = await CreateUserAsync();
        string jwt = GenerateJwtToken(user);
        
        HttpClient client = _app.CreateClient();
        client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer",jwt);
        client.DefaultRequestHeaders.Add(HeaderNames.Accept, "application/json");
        
        return (user, client);
    }

    private string GenerateJwtToken(AppUser user)
    {
        var claims = new List<Claim>
        {
            new Claim(ClaimTypes.Name, user.Id.ToString()),
            new Claim("gh-access-token", ""),
            new Claim("gh-refresh-token", "")
        };
        
        DateTime expiresAt = DateTime.UtcNow.AddHours(8);
        var signingCredentials = new SigningCredentials(_conf.GetJwtPrivateKey(), SecurityAlgorithms.RsaSha256);
        var token = new JwtSecurityToken(
            issuer: _conf.GetJwtIssuer(),
            audience: _conf.GetJwtAudience(),
            claims: claims,
            expires: expiresAt,
            signingCredentials: signingCredentials
        );
        
        return new JwtSecurityTokenHandler().WriteToken(token);
    }
}
