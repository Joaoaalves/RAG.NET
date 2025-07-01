using RAGNET.Domain.Chunkers;
using RAGNET.Domain.QueryEnhancers;
using RAGNET.Domain.QueryResultFilters;
using RAGNET.Domain.SharedKernel.Plans;
using RAGNET.Domain.SharedKernel.Plans.Policies;
using RAGNET.Domain.SharedKernel.Plans.Specifications;

namespace web.Configurations
{
    public static class SpecificationConfiguration
    {
        public static IServiceCollection AddSpecificationConfiguration(this IServiceCollection services)
        {
            services.AddSingleton<IAccessSpecification<ChunkerStrategy>, ChunkerAccessSpecification>();
            services.AddSingleton<IAccessSpecification<QueryEnhancerStrategy>, QueryEnhancerAccessSpecification>();
            services.AddSingleton<IAccessSpecification<QueryResultFilterStrategy>, QueryResultFiterAcessSpecification>();
            services.AddScoped<SubscriptionPolicy>();
            return services;
        }
    }
}