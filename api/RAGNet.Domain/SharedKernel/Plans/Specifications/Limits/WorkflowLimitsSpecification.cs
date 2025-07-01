using RAGNET.Domain.SharedKernel.Subscriptions;
using RAGNET.Domain.Workflows;

namespace RAGNET.Domain.SharedKernel.Plans.Specifications.Limits
{
    public class WorkflowLimitsSpecification : ILimitSpecification<Workflow>
    {
        public int GetLimit(PlanType plan) => plan switch
        {
            PlanType.Core => 5,
            PlanType.Enhanced => 15,
            PlanType.Ascend => int.MaxValue,
            _ => 0
        };
        public bool IsWithinLimit(PlanType plan, IReadOnlyCollection<Workflow> existingWorkflows)
        {
            var limit = GetLimit(plan);
            return existingWorkflows.Count < limit;
        }
    }
}