using RAGNET.Domain.Chunkers;
using RAGNET.Domain.SharedKernel.Subscriptions;

namespace RAGNET.Domain.SharedKernel.Plans.Specifications
{
    public class ChunkerAccessSpecification
    {
        private readonly PlanType _plan;

        public ChunkerAccessSpecification(PlanType plan)
        {
            _plan = plan;
        }

        public bool IsSatisfiedBy(ChunkerStrategy strategy)
        {
            return _plan switch
            {
                PlanType.Core => strategy == ChunkerStrategy.PARAGRAPH,
                PlanType.Enhanced => strategy == ChunkerStrategy.PARAGRAPH || strategy == ChunkerStrategy.PROPOSITION,
                PlanType.Ascend => true,
                _ => false
            };
        }
    }
}