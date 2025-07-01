using RAGNET.Application.Configuration.Commands;
using RAGNET.Domain.SharedKernel.Subscriptions;

namespace RAGNET.Application.Subscriptions.Commands.ChangeSubscriptionPlan
{
    public class ChangeSubscriptionPlanCommand(
        ChangePlanRequest request
    ) : UserAwareCommand<Unit>
    {
        public BillingPeriod BillingPeriod { get; } = request.BillingPeriod;
        public PlanType NewPlan { get; } = request.NewPlan;
    }
}