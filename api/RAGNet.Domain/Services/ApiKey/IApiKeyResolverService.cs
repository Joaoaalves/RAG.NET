using RAGNET.Domain.Enums;

namespace RAGNET.Domain.Services.ApiKey
{
    public interface IApiKeyResolverService
    {
        Task<string> ResolveForUserAsync(string userId, SupportedProvider provider);
        Task<string> ResolveForUserAsync(string userId, ConversationProviderEnum provider);
        Task<string> ResolveForUserAsync(string userId, EmbeddingProviderEnum provider);
    }
}