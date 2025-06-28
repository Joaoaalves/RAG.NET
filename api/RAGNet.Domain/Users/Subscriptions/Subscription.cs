using RAGNET.Domain.Chunkers;
using RAGNET.Domain.SeedWork;
using RAGNET.Domain.SharedKernel.Subscriptions;
using RAGNET.Domain.Users.Subscriptions.Events;

namespace RAGNET.Domain.Users.Subscriptions
{
    public class Subscription : Entity, IUserOwned
    {
        public SubscriptionId Id { get; private init; } = default!;
        public string UserId { get; set; } = null!;
        public SubscriptionPlan Plan { get; private set; } = null!;
        public string? PaymentId { get; private set; }
        public DateTime SubscribedAt { get; private set; }
        public DateTime ExpiresAt { get; private set; }

        // Payment Gateway
        public string? SubscriptionId { get; private set; }
        public SubscriptionPlan? ScheduledPlan { get; private set; }

        // EF Core CTOR
        private Subscription() { }

        private Subscription(SubscriptionId id, string userId, SubscriptionPlan plan)
        {

            var now = DateTime.UtcNow;
            var month = TimeSpan.FromDays(30);
            Id = id;
            UserId = userId;
            Plan = plan;
            SubscribedAt = now;
            ExpiresAt = plan.Value != PlanType.Core ? now + month : DateTime.MaxValue;

            AddDomainEvent(new SubscriptionCreatedEvent(UserId, Plan, ExpiresAt));
        }

        public static Subscription Create(string userId, SubscriptionPlan? plan = null, SubscriptionId? id = null)
        {
            return new Subscription(
                id ?? new SubscriptionId(),
                userId,
                plan ?? SubscriptionPlan.Core
            );
        }

        public bool IsActive()
        {
            var isActive = ExpiresAt > DateTime.UtcNow;

            if (!isActive)
            {
                Expire();
            }

            return isActive;
        }

        public void Renew(SubscriptionPlan plan, string paymentId, string subscriptionId)
        {
            var now = DateTime.UtcNow;
            var month = TimeSpan.FromDays(30);

            Plan = ScheduledPlan ?? plan;
            ScheduledPlan = null;
            SubscribedAt = now;
            PaymentId = paymentId;
            SubscriptionId = subscriptionId;
            ExpiresAt = now + month;

            AddDomainEvent(new SubscriptionRenewedEvent(UserId, subscriptionId, PaymentId, Plan, ExpiresAt));
        }
        public void SchedulePlanChange(SubscriptionPlan newPlan)
        {
            if (newPlan.Value == Plan.Value)
                throw new InvalidOperationException("No plan changes.");

            ScheduledPlan = newPlan;
        }

        public void Expire()
        {
            Plan = SubscriptionPlan.Core;
            SubscribedAt = DateTime.UtcNow;
            ExpiresAt = DateTime.MaxValue;

            AddDomainEvent(new SubscriptionExpiredEvent(this));
        }
        public bool AllowsChunker(ChunkerStrategy strategy)
        {
            return IsActive() && Plan.Allows(strategy);
        }

    }

}