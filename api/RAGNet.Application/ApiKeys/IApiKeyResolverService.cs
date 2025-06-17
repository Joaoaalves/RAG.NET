using RAGNET.Domain.SharedKernel.Providers;

namespace RAGNET.Application.ApiKeys
{
    public interface IApiKeyResolverService
    {
        Task<string> ResolveForUserAsync(string userId, SupportedProvider provider);
        Task<string> ResolveForUserAsync(string userId, ConversationProviderEnum provider);
        Task<string> ResolveForUserAsync(string userId, EmbeddingProviderEnum provider);
    }
}