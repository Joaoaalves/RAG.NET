using RAGNET.Domain.SeedWork;

namespace RAGNET.Domain.Users.Subscriptions.Events
{
    public class SubscriptionCanceledEvent(Subscription subscription) : DomainEventBase
    {
        public Subscription Subscription { get; } = subscription;
    }
}