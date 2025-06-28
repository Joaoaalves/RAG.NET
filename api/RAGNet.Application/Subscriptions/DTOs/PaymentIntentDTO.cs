using RAGNET.Domain.SharedKernel.Subscriptions;

namespace RAGNET.Application.Subscriptions.DTOs
{
    public class PaymentIntentDTO
    {
        public string UserId { get; set; } = string.Empty;
        public string PaymentIntentId { get; set; } = string.Empty;
        public string SubscriptionId { get; set; } = string.Empty;
        public string PlanType { get; set; } = string.Empty;
    }
}