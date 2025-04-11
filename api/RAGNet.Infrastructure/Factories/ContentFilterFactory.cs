using RAGNET.Domain.Entities;
using RAGNET.Domain.Enums;
using RAGNET.Domain.Factories;
using RAGNET.Domain.Services;
using RAGNET.Domain.Services.Filter;
using RAGNET.Infrastructure.Adapters.Filter;

namespace RAGNET.Infrastructure.Factories
{
    public class ContentFilterFactory(IPromptService promptService) : IContentFilterFactory
    {
        private readonly IPromptService _promptService = promptService;
        public IContentFilterService CreateContentFilter(Filter filter)
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
                FilterStrategy.RELEVANT_SEGMENT_EXTRACTION => new RSEFilter(
                    _promptService.GetPrompt("Filters", "rse"),
                    maximumItems
                ),
                _ => throw new NotSupportedException("Content filter not supported")
            };
        }
    }
}