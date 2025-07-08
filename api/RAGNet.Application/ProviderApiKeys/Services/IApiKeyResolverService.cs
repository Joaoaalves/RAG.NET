using RAGNET.Domain.SharedKernel.Providers;
using RAGNET.Domain.Users.ApiKeys;

namespace RAGNET.Application.ProviderApiKeys.Services
{
    public interface IApiKeyResolverService
    {
        Task<ApiKey> ResolveForUserAsync(string userId, SupportedProvider provider);
        Task<ApiKey> ResolveForUserAsync(string userId, ConversationProviderEnum provider);
        Task<ApiKey> ResolveForUserAsync(string userId, EmbeddingProviderEnum provider);
    }
}