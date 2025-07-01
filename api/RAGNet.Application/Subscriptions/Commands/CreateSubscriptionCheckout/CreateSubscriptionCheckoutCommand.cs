using RAGNET.Application.Configuration.Commands;
using RAGNET.Application.Subscriptions.DTOs;
using RAGNET.Domain.SharedKernel.Subscriptions;

namespace RAGNET.Application.Subscriptions.Commands.CreateSubscriptionCheckout
{
    public class CreateSubscriptionCheckoutCommand(
        StartCheckoutRequest dto
    ) : UserAwareCommand<string>
    {
        public PlanType PlanType { get; } = dto.PlanType;
        public BillingPeriod BillingPeriod { get; } = dto.BillingPeriod;
        public string SuccessUrl { get; init; } = dto.SuccessUrl;
        public string CancelUrl { get; init; } = dto.CancelUrl;
    }
}