
using RAGNET.Domain.QueryEnhancers;

using RAGNET.Application.Providers;
using RAGNET.Application.UserQueries;

namespace RAGNET.Application.UserQueriesEnhancers
{
    public interface IQueryEnhancerFactory
    {
        IQueryEnhancerService CreateQueryEnhancer(QueryEnhancer enhancerConfig, IChatCompletionService completionService);
    }
}