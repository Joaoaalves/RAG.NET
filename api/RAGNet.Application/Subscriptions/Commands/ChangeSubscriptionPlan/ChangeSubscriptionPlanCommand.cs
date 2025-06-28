using RAGNET.Application.Configuration.Commands;
using RAGNET.Domain.SharedKernel.Subscriptions;

namespace RAGNET.Application.Subscriptions.Commands.ChangeSubscriptionPlan
{
    public class ChangeSubscriptionPlanCommand(
        PlanType newPlan
    ) : UserAwareCommand<Unit>
    {
        public PlanType NewPlan { get; } = newPlan;
    }
}