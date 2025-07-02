using RAGNET.Domain.SeedWork;

namespace RAGNET.Domain.Users.Subscriptions.Events
{
    public class SubscriptionExpiredEvent : DomainEventBase
    {
        public Subscription Subscription { get; }
        public SubscriptionExpiredEvent(Subscription subscription)
        {
            Subscription = subscription;
            Console.WriteLine($"Subscription Expired for User: {Subscription.UserId}");
        }
    }
}