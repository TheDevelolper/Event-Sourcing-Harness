using System.Data;
using System.Net.Http.Headers;
using Marten;
using Marten.Events;
using Mediator.Net;
using Mediator.Net.MicrosoftDependencyInjection;
using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.DependencyInjection;
using Moq;
using SaasFactory.Features.UserSubscriptions.Contracts.Events;
using Serilog;
using TestStack.BDDfy;

namespace SaasFactory.Features.UserSubscriptions.Tests.Specifications;

[TestFixture]
public class UserSubscriptionSpecs : SpecsBase
{
    private HttpClient _httpClient;
    private readonly Mock<IEventStoreOperations> _mockEvents = new();
    private readonly Mock<IDocumentSession> _mockMartenDocumentSession = new();
    private readonly Mock<IDocumentStore> _mockMartenDocumentStore = new();

    [SetUp]
    public async Task SetUpAsync()
    {
        _httpClient = await GetFakeHttpClientAsync();

        _mockEvents
            .Setup(e => e.StartStream(
                It.IsAny<Guid>(),
                It.IsAny<SubscriptionPendingEvent>()));

        _mockMartenDocumentSession
            .Setup(s => s.Events)
            .Returns(_mockEvents.Object);

        _mockMartenDocumentSession
            .Setup(s => s.SaveChangesAsync(It.IsAny<CancellationToken>()))
            .Returns(Task.CompletedTask);

        _mockMartenDocumentStore
            .Setup(m => m.LightweightSession(It.IsAny<IsolationLevel>()))
            .Returns(_mockMartenDocumentSession.Object);
    }

    [OneTimeTearDown]
    public void OneTimeTearDown()
    {
        _httpClient.Dispose();
    }

    [Test]
    public void StartingSubscriptionProcess()
    {
        // # Creating a new subscription
        //   CreateSubscriptionRequest
        //     InitiateSubscriptionCommand
        //     SubscriptionPendingEvent
        //        PlaceOrderCommand
        //        OrderPendingEvent
        //          CreatePaymentCommand
        //          PaymentPendingCommand
        //             -> paymentSystem
        //          PaymentProcessedEvent
        //        OrderCompletedEvent
        //     SubscriptionActivatedEvent
        //   CreateSubscriptionResponse

        this.Given(_ => ARegisteredUser())
            .When(_ => TheUserSubscribesToAPlan())
            .Then(_ => TheSubscriptionIsPending())
            .BDDfy("Starting the subscription process");
    }

    private void ARegisteredUser()
    {
        var fakeJwt = GenerateTestJwt(
            username: "jane.doe",
            email: "jane.doe@example.com",
            lifetime: TimeSpan.FromMinutes(30));

        _httpClient.DefaultRequestHeaders.Authorization =
            new AuthenticationHeaderValue("Bearer", fakeJwt);
    }

    private async Task TheUserSubscribesToAPlan()
    {
        _httpClient.GetAsync("/subscribe").Result.EnsureSuccessStatusCode();

        mockLogger.Verify(m =>
            m.Information("User {0}, made a subscription request", "jane.doe"), Times.Once);
    }

    private Task TheSubscriptionIsPending()
    {
        _mockEvents.Verify(m =>
                m.StartStream(It.IsAny<Guid>(), It.IsAny<SubscriptionPendingEvent>()),
                Times.Once);

        _mockMartenDocumentSession.Verify(m =>
            m.SaveChangesAsync(It.IsAny<CancellationToken>()),
            Times.Once);

        return Task.CompletedTask;
    }

    private void ThePaymentIsProcessedSuccessfully()
    {
    }

    private void TheUserSubscriptionShouldBeActivated()
    {
    }

    protected override void RegisterServices(IServiceCollection services)
    {
        services.AddSingleton(mockLogger.Object);
        services.AddSingleton(_mockMartenDocumentStore.Object);

        var mediaBuilder = new MediatorBuilder();
        mediaBuilder.RegisterHandlers(typeof(IUserSubscriptionMarker).Assembly).Build();
        services.RegisterMediator(mediaBuilder);
    }

    protected override void MapEndpoints(WebApplication app)
    {
        app.MapUserSubscriptionEndpoints(mockLogger.Object);
    }
}

public class Currency
{
    public const string USD = nameof(USD);
    public const string GBP = nameof(GBP);
    public const string EUR = nameof(EUR);
}

// CreatePendingSubscriptionOrderCommandb
// subscription:
//   product: "Menuota"
//   tier: Pro/Noob
//   billingCycle: Annual/Monthly
// order:
//   customerId:
//   orderNumber: "#1234",
//   orderUtcDateTime: 
//   items: [{name, quantity, price, billingCycle}], // subscription details for subscription
//   description: "{subscription.billingCycle} subscription for {subscription.product} {subscription.tier}"
//   payment: 
//   
// _mockMediator.Verify(m => m.SendAsync(
//     new CreateSubscriptionOrderCommand(
//         orderData: OrderData.New(
//             
//             username:  "jane.doe", 
//             orderNumber: "Order #1234",
//             paymentReason: "Subscription to pro plan",
//             DateTimeUTC: It.IsAny<DateTime>(),
//             notes: null
//         ),
//         subscriptionData: SubscriptionData.New(
//             subscriptionTier: subscriptionTier.Pro,
//             billingCycle: BillingCycle.Annual
//         ),
//         paymentData: PaymentData.New(
//             idempotencyKey: It.IsAny<Guid>(),
//             amount: 12.50, // including tax with discount applied
//             sourceId: It.IsAny<string>(), 
//             currency: It.IsIn([
//                 Currency.USD,
//                 Currency.EUR,
//                 Currency.GBP
//             ])
//         ),
//         pricingData: PricingData.New(
//             taxAmount: 1.25,
//             discountCode: "NEWYEAR2025",
//             discountAmount: 2.00
//         ),
//         metadata: MessageMetadata.New(
//             //Helps when events span multiple bounded contexts (like Orders → Payments → Notifications)
//             correlationId: Guid.NewGuid(), 
//             sourceSystem: "WebApp",
//             requestOrigin: "UserPortal"
//         )
//     ), Times.Once)
// );