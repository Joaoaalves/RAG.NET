
using RAGNET.Domain.QueryEnhancers;

using RAGNET.Application.Queries;
using RAGNET.Application.Providers.Conversation;

namespace RAGNET.Application.QueryEnhancers
{
    public interface IQueryEnhancerFactory
    {
        IQueryEnhancerService CreateQueryEnhancer(QueryEnhancer enhancerConfig, IChatCompletionService completionService);
    }
}