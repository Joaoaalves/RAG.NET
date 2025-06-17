
using RAGNET.Domain.QueryEnhancers;

using RAGNET.Application.Providers;
using RAGNET.Application.Query;

namespace RAGNET.Application.QueryEnhancers
{
    public interface IQueryEnhancerFactory
    {
        IQueryEnhancerService CreateQueryEnhancer(QueryEnhancer enhancerConfig, IChatCompletionService completionService);
    }
}