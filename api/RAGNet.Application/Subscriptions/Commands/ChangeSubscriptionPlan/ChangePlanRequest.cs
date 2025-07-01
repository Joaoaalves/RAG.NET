using RAGNET.Domain.SharedKernel.Subscriptions;

namespace RAGNET.Application.Subscriptions.Commands.ChangeSubscriptionPlan
{
    public class ChangePlanRequest
    {
        public BillingPeriod BillingPeriod { get; set; }
        public PlanType NewPlan { get; set; }
    }
}