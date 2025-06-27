using RAGNET.Domain.SeedWork;
using RAGNET.Domain.SharedKernel.Subscriptions;

namespace RAGNET.Domain.Users.Subscriptions.Events
{
    public class SubscriptionCreatedEvent : DomainEventBase
    {
        public string UserId { get; } = string.Empty;
        public SubscriptionPlan Plan { get; }
        public DateTime ExpiresAt { get; }

        public SubscriptionCreatedEvent(string userId, SubscriptionPlan plan, DateTime expiresAt)
        {
            UserId = userId;
            Plan = plan;
            ExpiresAt = expiresAt;

            Console.WriteLine($"Subscription Created for user: {UserId} with plan: {Plan.Value}");
        }
    }
}