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
        public SubscriptionStatus? Status { get; private set; }
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

        public void Renew(DateTime renewedAt, DateTime expiresAt, BillingPeriod billingPeriod)
        {
            SubscribedAt = renewedAt;
            ExpiresAt = expiresAt;

            AddDomainEvent(new SubscriptionRenewedEvent(this, billingPeriod));
        }

        public void AddSubscription(SubscriptionPlan plan, BillingPeriod billingPeriod, DateTime renewedAt, DateTime expiresAt, string paymentId, string subscriptionId)
        {
            Plan = plan;
            Status = SubscriptionStatus.Active;
            SubscribedAt = renewedAt;
            PaymentId = paymentId;
            SubscriptionId = subscriptionId;
            ExpiresAt = expiresAt;

            AddDomainEvent(new SubscriptionCreatedEvent(this, billingPeriod));
        }

        public void Cancel()
        {
            ClearPlan();
            AddDomainEvent(new SubscriptionCanceledEvent(this));
        }

        public bool AllowsChunker(ChunkerStrategy strategy)
        {
            return IsActive() && Plan.Allows(strategy);
        }

        private void Expire()
        {
            ClearPlan();

            AddDomainEvent(new SubscriptionExpiredEvent(this));
        }

        private void ClearPlan()
        {
            Plan = SubscriptionPlan.Core;
            SubscriptionId = null;
            SubscribedAt = DateTime.UtcNow;
            PaymentId = null;
            ExpiresAt = DateTime.MaxValue;
        }
    }

}