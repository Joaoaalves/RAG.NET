using RAGNET.Domain.QueryEnhancers;

using RAGNET.Application.Infrastructure.Providers.Conversation;
using RAGNET.Application.Queries.Services;

namespace RAGNET.Application.QueryEnhancers.Factories
{
    public interface IQueryEnhancerFactory
    {
        IQueryEnhancerService CreateQueryEnhancer(QueryEnhancer enhancerConfig, IConversationProviderService completionService);
    }
}