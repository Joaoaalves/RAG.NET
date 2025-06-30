
using RAGNET.Domain.SharedKernel.Subscriptions;

namespace RAGNET.Application.Subscriptions.DTOs
{
    public class PaymentIntentDTO
    {
        public string CustomerId { get; set; } = String.Empty;
        public string PaymentIntentId { get; set; } = String.Empty;
        public string SubscriptionId { get; set; } = String.Empty;
        public PlanType PlanType { get; set; }
        public DateTime RenewedAt { get; set; }
        public DateTime ExpiresAt { get; set; }
    }
}