using RAGNET.Application.Configuration.Commands;
using RAGNET.Application.Subscriptions.DTOs;
using RAGNET.Domain.SharedKernel.Subscriptions;

namespace RAGNET.Application.Subscriptions.Commands.ProcessSuccessfulSubscription
{
    public class ProcessSuccessfulSubscriptionCommand(
        PaymentIntentDTO paymentIntentDTO
    ) : BaseCommand<Unit>
    {
        public string CustomerId { get; } = paymentIntentDTO.CustomerId;
        public string PaymentId { get; } = paymentIntentDTO.PaymentIntentId;
        public string SubscriptionId { get; } = paymentIntentDTO.SubscriptionId;
        public PlanType PlanType { get; } = paymentIntentDTO.PlanType;
        public BillingPeriod BillingPeriod { get; } = paymentIntentDTO.BillingPeriod;
        public DateTime RenewedAt { get; } = paymentIntentDTO.RenewedAt;
        public DateTime ExpiresAt { get; } = paymentIntentDTO.ExpiresAt;
    }
}