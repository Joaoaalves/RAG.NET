using RAGNET.Domain.Chunkers;
using RAGNET.Domain.SharedKernel.Subscriptions;

namespace RAGNET.Domain.SharedKernel.Plans.Specifications.Access
{
    public class ChunkerAccessSpecification : IAccessSpecification<ChunkerStrategy>
    {
        public bool IsSatisfiedBy(PlanType plan, ChunkerStrategy strategy)
        {
            return plan switch
            {
                PlanType.Core => strategy == ChunkerStrategy.PARAGRAPH,
                PlanType.Enhanced => strategy == ChunkerStrategy.PARAGRAPH || strategy == ChunkerStrategy.PROPOSITION,
                PlanType.Ascend => true,
                _ => false
            };
        }
    }
}