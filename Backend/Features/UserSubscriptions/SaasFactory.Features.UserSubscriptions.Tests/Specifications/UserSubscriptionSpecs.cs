using System.Net.Http.Headers;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.TestHost;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.Extensions.DependencyInjection;
using Moq;
using Serilog;
using TestStack.BDDfy;

namespace SaasFactory.Features.UserSubscriptions.Tests.Specifications;

[TestFixture]
public class UserSubscriptionSpecs: SpecsBase
{
    private HttpClient _httpClient;
    
    [SetUp]
    public async Task SetUpAsync()
    {
        _httpClient = await GetFakeHttpClientAsync();
    }

    [TearDown]
    public void TearDown()
    {
        _httpClient.Dispose();
    }
    
    [Test]
    public void UserSuccessfullySubscribesToAPlan()
    {
        this.Given(_ => ARegisteredUserId())
            .When(_ => TheUserSubscribesToAPlan())
            .And(_ => ThePaymentIsProcessedSuccessfully())
            .Then(_ => TheUserSubscriptionShouldBeActivated())
            .BDDfy("User Subscription success scenario");
    }

    private void ARegisteredUserId()
    {
        var fakeJwt = GenerateTestJwt(
            username: "jane.doe",
            email: "jane.doe@example.com",
            lifetime: TimeSpan.FromMinutes(30));

        _httpClient.DefaultRequestHeaders.Authorization =
            new AuthenticationHeaderValue("Bearer", fakeJwt);
    }
    
    private void TheUserSubscribesToAPlan()
    {
        _httpClient.GetAsync("/subscribe").Result.EnsureSuccessStatusCode();
        mockLogger.Verify(m => 
            m.Information("User {0}, subscribed to thing", "jane.doe"), Times.Once);
    }

    private void ThePaymentIsProcessedSuccessfully()
    {
        
    }

    private void TheUserSubscriptionShouldBeActivated()
    {
        
    }

    protected override void RegisterServices(IServiceCollection services)
    {
        var mockPaymentProcessor = new Mock<IPaymentProcessor>();
        services.AddSingleton<IPaymentProcessor>(mockPaymentProcessor.Object);
    }
    
    protected override WebApplication MapEndpoints(WebApplication app)
    {
        app.MapUserSubscriptionEndpoints(mockLogger.Object);
        return app;
    }

}

internal interface IPaymentProcessor
{
}