using RAGNET.Domain.SeedWork;

namespace RAGNET.Domain.Users.Subscriptions.Events
{
    public class SubscriptionRenewedEvent : DomainEventBase
    {
        public Subscription Subscription { get; }
        public SubscriptionRenewedEvent(Subscription subscription)
        {
            Subscription = subscription;

            Console.WriteLine($"Subscription Renewed for user: {Subscription.UserId} with plan: {Subscription.Plan.Value}");
        }
    }
}