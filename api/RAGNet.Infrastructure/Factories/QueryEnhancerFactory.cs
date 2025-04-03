using RAGNET.Domain.Entities;
using RAGNET.Domain.Enums;
using RAGNET.Domain.Factories;
using RAGNET.Domain.Services;
using RAGNET.Infrastructure.Adapters.QueryEnhancer;

namespace RAGNET.Infrastructure.Factories
{
    public class QueryEnhancerFactory : IQueryEnhancerFactory
    {
        public IQueryEnhancerService CreateQueryEnhancer(QueryEnhancer enhancerConfig, IChatCompletionService completionService)
        {
            return enhancerConfig.Type switch
            {
                QueryEnhancerStrategy.HYPOTHETICAL_DOCUMENT_EMBEDDING => new HYDEAdapter(enhancerConfig.MaxQueries, completionService),
                QueryEnhancerStrategy.SELF_QUERYING_RETRIEVAL => new SQRAdapter(enhancerConfig.MaxQueries, completionService),
                QueryEnhancerStrategy.AUTO_QUERY => new AutoQueryAdapter(enhancerConfig.MaxQueries, completionService),
                _ => throw new NotSupportedException("Query Enhancer Strategy not supported.")
            };
        }
    }
}