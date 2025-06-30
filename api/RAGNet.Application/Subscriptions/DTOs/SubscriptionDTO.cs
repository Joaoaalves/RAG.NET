using RAGNET.Domain.SharedKernel.Subscriptions;

namespace RAGNET.Application.Subscriptions.DTOs
{
    public class SubscriptionDTO
    {
        public required PlanType PlanType { get; set; }
        public required DateTime SubscribedAt { get; set; }
        public required DateTime ExpiresAt { get; set; }
        public SubscriptionStatus? Status { get; set; }
    }
}