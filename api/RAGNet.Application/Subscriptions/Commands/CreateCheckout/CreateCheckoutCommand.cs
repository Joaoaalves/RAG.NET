using RAGNET.Application.Configuration.Commands;
using RAGNET.Domain.SharedKernel.Subscriptions;

namespace RAGNET.Application.Subscriptions.Commands.CreateCheckout
{
    public class CreateSubscriptionCheckoutCommand(
        PlanType planType,
        string successUrl,
        string cancelUrl
    ) : UserAwareCommand<string>
    {
        public PlanType PlanType { get; } = planType;
        public string SuccessUrl { get; init; } = successUrl;
        public string CancelUrl { get; init; } = cancelUrl;
    }
}