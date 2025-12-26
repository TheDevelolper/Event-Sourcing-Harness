using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Routing;
using Microsoft.AspNetCore.TestHost;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.IdentityModel.Tokens;
using Moq;
using Serilog;

namespace SaasFactory.UserSubscriptions.Tests.Specifications;

public abstract class SpecsBase
{
    private const string TestIssuer = "https://test-issuer";
    private const string TestAudience = "test-audience";
    private const string TestSigningKey = "super-secret-test-key-which-is-long"; // >= 32 chars
    private WebApplicationBuilder _builder;
    protected readonly Mock<ILogger> MockLogger = new();

    protected static string GenerateTestJwt(string username, string email, TimeSpan lifetime)
    {
        var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(TestSigningKey));
        var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);

        var now = DateTime.UtcNow;

        var token = new JwtSecurityToken(
            issuer: TestIssuer,
            audience: TestAudience,
            claims:
            [
                new Claim("preferred_username", username),
                new Claim(ClaimTypes.Email, email),
                new Claim(ClaimTypes.NameIdentifier, Guid.NewGuid().ToString())
            ],
            notBefore: now,
            expires: now.Add(lifetime),
            signingCredentials: creds);

        return new JwtSecurityTokenHandler().WriteToken(token);
    }

    private static void ConfigureJwtForTests(JwtBearerOptions options)
    {
        options.TokenValidationParameters = new TokenValidationParameters
        {
            ValidIssuer = TestIssuer,
            ValidAudience = TestAudience,
            ValidateIssuer = true,
            ValidateLifetime = true,
            ValidateAudience = true,
            ValidateIssuerSigningKey = true,
            IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(TestSigningKey)),
            ClockSkew = TimeSpan.Zero,
            NameClaimType = "preferred_username" // so User.Identity.Name works
        };
    }
    
    protected async Task<HttpClient> GetFakeHttpClientAsync()
    {
        _builder = WebApplication.CreateBuilder();
        _builder.Environment.EnvironmentName = "Testing";
        _builder.WebHost.UseTestServer();
        
        _builder.Services
            .AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
            .AddJwtBearer(ConfigureJwtForTests);
        
        _builder.Services.AddAuthorization();
        RegisterServices(_builder.Services);
        
        var app = _builder.Build();
        
        MapEndpoints(app);
        
        await app.StartAsync();
        var fakeHttpClient = app.GetTestClient();
        
        return fakeHttpClient;
    }

    protected virtual void MapEndpoints(WebApplication app)
    {
        
    }

    protected virtual void RegisterServices(IServiceCollection services)
    {
        
    }
}