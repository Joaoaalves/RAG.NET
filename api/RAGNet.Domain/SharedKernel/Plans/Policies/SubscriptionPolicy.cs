using RAGNET.Domain.Chunkers;
using RAGNET.Domain.QueryEnhancers;
using RAGNET.Domain.QueryResultFilters;
using RAGNET.Domain.Users;
using RAGNET.Domain.Workflows;

namespace RAGNET.Domain.SharedKernel.Plans.Policies
{
    public class SubscriptionPolicy(
        IAccessSpecification<ChunkerStrategy> chunkerAccessSpec,
        IAccessSpecification<QueryEnhancerStrategy> queryEnhancerAccessSpec,
        IAccessSpecification<QueryResultFilterStrategy> queryResultFilterAccessSpec,
        ILimitSpecification<Workflow> workflowLimitSpec
    )
    {

        private readonly IAccessSpecification<ChunkerStrategy> _chunkerAccessSpec = chunkerAccessSpec;

        private readonly IAccessSpecification<QueryEnhancerStrategy> _queryEnhancerAccessSpec = queryEnhancerAccessSpec;

        private readonly IAccessSpecification<QueryResultFilterStrategy> _queryResultFilterAccessSpec = queryResultFilterAccessSpec;

        private readonly ILimitSpecification<Workflow> _workflowLimitSpec = workflowLimitSpec;
        public bool Allows(User user, ChunkerStrategy strategy)
        {
            var plan = user.Subscription.Plan.Value;

            return _chunkerAccessSpec.IsSatisfiedBy(plan, strategy);
        }

        public bool Allows(User user, QueryEnhancerStrategy strategy)
        {
            var plan = user.Subscription.Plan.Value;
            return _queryEnhancerAccessSpec.IsSatisfiedBy(plan, strategy);
        }

        public bool Allows(User user, QueryResultFilterStrategy strategy)
        {
            var plan = user.Subscription.Plan.Value;
            return _queryResultFilterAccessSpec.IsSatisfiedBy(plan, strategy);
        }

        public bool AllowsWorkflowCreation(User user)
        {
            var plan = user.Subscription.Plan.Value;

            return _workflowLimitSpec.IsWithinLimit(plan, user.Workflows);
        }
    }
}
