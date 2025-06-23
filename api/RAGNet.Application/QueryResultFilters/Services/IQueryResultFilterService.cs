using RAGNET.Application.Infrastructure.Providers.Conversation;
using RAGNET.Application.Queries.DTOs;

namespace RAGNET.Application.QueryResultFilters.Services
{
    public interface IQueryResultFilterService
    {
        Task<List<string>> FilterContent(List<ContentItemDTO> contentItems, string query, IConversationProviderService completionProvider);
    }
}