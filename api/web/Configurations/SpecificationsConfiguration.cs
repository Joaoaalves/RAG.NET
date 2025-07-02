using RAGNET.Domain.Chunkers;
using RAGNET.Domain.QueryEnhancers;
using RAGNET.Domain.QueryResultFilters;
using RAGNET.Domain.SharedKernel.Plans;
using RAGNET.Domain.SharedKernel.Plans.Policies;
using RAGNET.Domain.SharedKernel.Plans.Specifications.Access;
using RAGNET.Domain.SharedKernel.Plans.Specifications.Limits;
using RAGNET.Domain.Workflows;

namespace web.Configurations
{
    public static class SpecificationConfiguration
    {
        public static IServiceCollection AddSpecificationConfiguration(this IServiceCollection services)
        {
            // Access Specs
            services.AddSingleton<IAccessSpecification<ChunkerStrategy>, ChunkerAccessSpecification>();
            services.AddSingleton<IAccessSpecification<QueryEnhancerStrategy>, QueryEnhancerAccessSpecification>();
            services.AddSingleton<IAccessSpecification<QueryResultFilterStrategy>, QueryResultFiterAcessSpecification>();
            services.AddSingleton<IAccessSpecification<ApiAccess>, ApiAccessSpecification>();
            services.AddSingleton<IAccessSpecification<WebhookAccess>, WebhookAccessSpecification>();

            // Limits Specs
            services.AddSingleton<ILimitSpecification<Workflow>, WorkflowLimitsSpecification>();

            // Policy
            services.AddSingleton<IFileSizePolicy, FileSizePolicy>();
            services.AddScoped<SubscriptionPolicy>();

            return services;
        }
    }
}