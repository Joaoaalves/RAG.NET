using RAGNET.Domain.QueryEnhancers;

using RAGNET.Application.Queries;
using RAGNET.Application.Providers;
using RAGNET.Application.Providers.Conversation;
using RAGNET.Application.QueryEnhancers.Strategies;

namespace RAGNET.Application.QueryEnhancers
{
    public class QueryEnhancerFactory(IPromptService promptService) : IQueryEnhancerFactory
    {
        private readonly IPromptService _promptService = promptService;
        public IQueryEnhancerService CreateQueryEnhancer(QueryEnhancer enhancerConfig, IConversationProviderService completionService)
        {
            var guidance = "";
            if (enhancerConfig.Metas != null && enhancerConfig.Metas.Count != 0)
            {
                var metaDict = enhancerConfig.Metas.ToDictionary(m => m.Key, m => m.Value);
                if (metaDict.TryGetValue("guidance", out string? guidanceElement))
                    guidance = guidanceElement;

            }

            return enhancerConfig.Type switch
            {
                QueryEnhancerStrategy.HYPOTHETICAL_DOCUMENT_EMBEDDING => new HYDEStrategy
                (
                    _promptService.GetPrompt("QueryEnhancers", "hyde"),
                    enhancerConfig.MaxQueries,
                    completionService
                ),
                QueryEnhancerStrategy.AUTO_QUERY => new AutoQueryStrategy
                (
                    _promptService.GetPrompt("QueryEnhancer", "autoQuery"),
                    enhancerConfig.MaxQueries,
                    guidance,
                    completionService
                ),
                _ => throw new NotSupportedException("Query Enhancer Strategy not supported.")
            };
        }
    }
}