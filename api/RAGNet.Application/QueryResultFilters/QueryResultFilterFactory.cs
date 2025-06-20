using RAGNET.Domain.QueryResultFilters;

using RAGNET.Application.Providers;
using RAGNET.Application.QueryResultFilters.Strategies;

namespace RAGNET.Application.QueryResultFilters
{
    public class QueryResultFilterFactory(IPromptService promptService) : IQueryResultFilterFactory
    {
        private readonly IPromptService _promptService = promptService;
        public IQueryResultFilterService CreateQueryResultFilter(QueryResultFilter filter)
        {
            int maximumItems = 5;
            if (filter.Metas != null && filter.Metas.Count != 0)
            {
                var metaDict = filter.Metas.ToDictionary(m => m.Key, m => m.Value);
                if (metaDict.TryGetValue("maximumItems", out string? topkElement))
                    maximumItems = Int32.Parse(topkElement);
            }

            return filter.Strategy switch
            {
                QueryResultFilterStrategyEnum.RELEVANT_SEGMENT_EXTRACTION => new RSEFilterStrategy(
                    _promptService.GetPrompt("Filters", "rse"),
                    maximumItems
                ),
                _ => throw new NotSupportedException("Content filter not supported")
            };
        }
    }
}