using RAGNET.Domain.SeedWork;
using RAGNET.Domain.SharedKernel.Subscriptions;

namespace RAGNET.Domain.Users.Subscriptions.Events
{
    public class SubscriptionRenewedEvent : DomainEventBase
    {
        public Subscription Subscription { get; }
        public BillingPeriod BillingPeriod { get; }
        public SubscriptionRenewedEvent(Subscription subscription, BillingPeriod billingPeriod)
        {
            Subscription = subscription;
            BillingPeriod = billingPeriod;
            Console.WriteLine($"Subscription Renewed for user: {Subscription.UserId} with plan: {Subscription.Plan.Value}");
        }
    }
}