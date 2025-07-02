using RAGNET.Domain.SharedKernel.Subscriptions;

namespace RAGNET.Application.Subscriptions.Commands.CreateSubscriptionCheckout
{
    public class StartCheckoutRequest
    {
        public PlanType PlanType { get; set; }
        public BillingPeriod BillingPeriod { get; set; }
        public string SuccessUrl { get; set; } = string.Empty;
        public string CancelUrl { get; set; } = string.Empty;
    }
}