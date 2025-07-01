using RAGNET.Domain.Chunkers;
using RAGNET.Domain.SharedKernel.Plans.Specifications;
using RAGNET.Domain.Users;

namespace RAGNET.Domain.SharedKernel.Plans.Policies
{
    public static class SubscriptionPolicy
    {
        public static bool AllowsChunker(User user, ChunkerStrategy strategy)
        {
            var plan = user.Subscription.Plan.Value;
            var spec = new ChunkerAccessSpecification(plan);
            return spec.IsSatisfiedBy(strategy);
        }
    }
}
