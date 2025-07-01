using RAGNET.Domain.QueryResultFilters;
using RAGNET.Domain.SharedKernel.Subscriptions;

namespace RAGNET.Domain.SharedKernel.Plans.Specifications
{
    public class QueryResultFiterAcessSpecification : IAccessSpecification<QueryResultFilterStrategy>
    {
        public bool IsSatisfiedBy(PlanType plan, QueryResultFilterStrategy strategy)
        {
            return plan switch
            {
                PlanType.Core => false,
                PlanType.Enhanced => true,
                PlanType.Ascend => true,
                _ => false
            };
        }
    }
}