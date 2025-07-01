using RAGNET.Domain.Chunkers;
using RAGNET.Domain.QueryEnhancers;
using RAGNET.Domain.SharedKernel.Subscriptions;

namespace RAGNET.Domain.SharedKernel.Plans.Specifications
{
    public class QueryEnhancerAccessSpecification : IAccessSpecification<QueryEnhancerStrategy>
    {
        public bool IsSatisfiedBy(PlanType plan, QueryEnhancerStrategy strategy)
        {
            return plan switch
            {
                PlanType.Core => strategy == QueryEnhancerStrategy.HYPOTHETICAL_DOCUMENT_EMBEDDING,
                PlanType.Enhanced => true,
                PlanType.Ascend => true,
                _ => false
            };
        }
    }
}