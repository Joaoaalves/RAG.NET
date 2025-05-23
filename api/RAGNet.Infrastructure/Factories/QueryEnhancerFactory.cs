using RAGNET.Domain.Entities;
using RAGNET.Domain.Enums;
using RAGNET.Domain.Factories;
using RAGNET.Domain.Services;
using RAGNET.Domain.Services.Query;
using RAGNET.Infrastructure.Adapters.QueryEnhancer;

namespace RAGNET.Infrastructure.Factories
{
    public class QueryEnhancerFactory(IPromptService promptService) : IQueryEnhancerFactory
    {
        private readonly IPromptService _promptService = promptService;
        public IQueryEnhancerService CreateQueryEnhancer(QueryEnhancer enhancerConfig, IChatCompletionService completionService)
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
                QueryEnhancerStrategy.HYPOTHETICAL_DOCUMENT_EMBEDDING => new HYDEAdapter
                (
                    _promptService.GetPrompt("QueryEnhancers", "hyde"),
                    enhancerConfig.MaxQueries,
                    completionService
                ),
                QueryEnhancerStrategy.AUTO_QUERY => new AutoQueryAdapter
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