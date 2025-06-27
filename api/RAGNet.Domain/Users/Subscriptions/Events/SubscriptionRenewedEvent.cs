using RAGNET.Domain.SeedWork;
using RAGNET.Domain.SharedKernel.Subscriptions;

namespace RAGNET.Domain.Users.Subscriptions.Events
{
    public class SubscriptionRenewedEvent : DomainEventBase
    {
        public string UserId { get; } = string.Empty;
        public SubscriptionPlan Plan { get; }
        public DateTime ExpiresAt { get; }

        public string PaymentId { get; }

        public SubscriptionRenewedEvent(string userId, string paymentId, SubscriptionPlan plan, DateTime expiresAt)
        {
            UserId = userId;
            PaymentId = paymentId;
            Plan = plan;
            ExpiresAt = expiresAt;

            Console.WriteLine($"Subscription Renewed for user: {UserId} with plan: {Plan.Value}");
        }
    }
}