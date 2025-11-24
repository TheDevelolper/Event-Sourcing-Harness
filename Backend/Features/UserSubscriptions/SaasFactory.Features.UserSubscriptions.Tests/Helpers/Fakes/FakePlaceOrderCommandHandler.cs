using Mediator.Net.Context;
using Mediator.Net.Contracts;
using SaasFactory.Features.UserSubscriptions.Contracts.Commands;

namespace SaasFactory.Features.UserSubscriptions.Tests.Specifications;

internal class FakePlaceOrderCommandHandler : ICommandHandler<PlaceOrderCommand>
{
    // Spy/verification fields
    public static int CallCount { get; private set; } = 0;
    public static PlaceOrderCommand? LastCommand { get; private set; }

    public Task Handle(IReceiveContext<PlaceOrderCommand> context, CancellationToken cancellationToken)
    {
        CallCount++;
        LastCommand = context.Message;
        return Task.CompletedTask;
    }

    public static void Reset()
    {
        CallCount = 0;
        LastCommand = null;
    }
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