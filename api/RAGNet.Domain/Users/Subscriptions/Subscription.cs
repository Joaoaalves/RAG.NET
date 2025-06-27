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
            ExpiresAt = now + month;

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

        public bool IsActive() =>
            ExpiresAt > DateTime.UtcNow;

        public void Renew(SubscriptionPlan plan, string paymentId)
        {
            var now = DateTime.UtcNow;
            var month = TimeSpan.FromDays(30);

            Plan = plan;
            PaymentId = paymentId;
            SubscribedAt = now;
            ExpiresAt = now + month;

            AddDomainEvent(new SubscriptionRenewedEvent(UserId, PaymentId, Plan, ExpiresAt));
        }

        public bool AllowsChunker(ChunkerStrategy strategy)
        {
            return IsActive() && Plan.Allows(strategy);
        }

    }

}