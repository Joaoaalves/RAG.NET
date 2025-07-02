using RAGNET.Domain.Chunkers;
using RAGNET.Domain.QueryEnhancers;
using RAGNET.Domain.QueryResultFilters;
using RAGNET.Domain.SharedKernel.Plans.Specifications.Access;
using RAGNET.Domain.Users;
using RAGNET.Domain.Workflows;

namespace RAGNET.Domain.SharedKernel.Plans.Policies
{
    public class SubscriptionPolicy(
        IAccessSpecification<ChunkerStrategy> chunkerAccessSpec,
        IAccessSpecification<QueryEnhancerStrategy> queryEnhancerAccessSpec,
        IAccessSpecification<QueryResultFilterStrategy> queryResultFilterAccessSpec,
        IAccessSpecification<ApiAccess> apiAccessSpecification,
        IAccessSpecification<WebhookAccess> webhookAccessSpecification,
        ILimitSpecification<Workflow> workflowLimitSpec,
        IFileSizePolicy fileSizePolicy
    )
    {
        private readonly IAccessSpecification<ChunkerStrategy> _chunkerAccessSpec = chunkerAccessSpec;
        private readonly IAccessSpecification<QueryEnhancerStrategy> _queryEnhancerAccessSpec = queryEnhancerAccessSpec;
        private readonly IAccessSpecification<QueryResultFilterStrategy> _queryResultFilterAccessSpec = queryResultFilterAccessSpec;
        private readonly IAccessSpecification<ApiAccess> _apiAccessSpecification = apiAccessSpecification;
        private readonly IAccessSpecification<WebhookAccess> _webhookAccessSpecification = webhookAccessSpecification;
        private readonly ILimitSpecification<Workflow> _workflowLimitSpec = workflowLimitSpec;
        private readonly IFileSizePolicy _fileSizePolicy = fileSizePolicy;

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

        public bool AllowsFileSize(User user, long fileSize)
        {
            var plan = user.Subscription.Plan.Value;
            return _fileSizePolicy.IsSatisfiedBy(plan, fileSize);
        }

        public bool AllowsApiUsage(User user)
        {
            var plan = user.Subscription.Plan.Value;
            return _apiAccessSpecification.IsSatisfiedBy(plan, ApiAccess.Required);
        }

        public bool AllowsWebhookUsage(User user)
        {
            var plan = user.Subscription.Plan.Value;
            return _webhookAccessSpecification.IsSatisfiedBy(plan, WebhookAccess.Required);
        }
    }
}
