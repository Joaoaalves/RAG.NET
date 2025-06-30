using RAGNET.Domain.SeedWork;

namespace RAGNET.Domain.Users.Subscriptions.Events
{
    public class SubscriptionCreatedEvent : DomainEventBase
    {

        public Subscription Subscription { get; }

        public SubscriptionCreatedEvent(Subscription subscription)
        {
            Subscription = subscription;

            Console.WriteLine($"Subscription Created for user: {Subscription.UserId} with plan: {Subscription.Plan.Value}");
        }
    }
}