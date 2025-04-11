using RAGNET.Domain.Entities;

namespace RAGNET.Domain.Services.Filter
{
    public interface IContentFilterService
    {
        Task<List<string>> FilterContent(List<ContentItem> contentItems, string query, IChatCompletionService completionProvider);
    }
}