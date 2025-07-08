using RAGNET.Application.Infrastructure.Providers.Conversation.Mappers;
using RAGNET.Application.Infrastructure.Providers.Embedding.Mappers;
using RAGNET.Domain.ProvidersApiKeys;
using RAGNET.Domain.SharedKernel.Providers;
using RAGNET.Domain.Users.ApiKeys;

namespace RAGNET.Application.ProviderApiKeys.Services
{
    public class ApiKeyResolverService(IProviderApiKeyRepository providerApiKeyRepository, ICryptoService cryptoService) : IApiKeyResolverService
    {
        private readonly IProviderApiKeyRepository _providerApiKeyRepository = providerApiKeyRepository;
        private readonly ICryptoService _cryptoService = cryptoService;

        public async Task<ApiKey> ResolveForUserAsync(string userId, SupportedProvider provider)
        {
            var userApiKey = await _providerApiKeyRepository.GetByUserIdAndProviderAsync(userId, provider) ?? throw new Exception($"Api Key not found for provider: {provider}");

            // Decrypt
            var apiKey = _cryptoService.Decrypt(userApiKey.Provider.ApiKey.Value);

            return new ApiKey(apiKey);
        }

        public Task<ApiKey> ResolveForUserAsync(string userId, ConversationProviderEnum provider)
        {
            return ResolveForUserAsync(userId, provider.ToSupportedProvider());
        }

        public Task<ApiKey> ResolveForUserAsync(string userId, EmbeddingProviderEnum provider)
        {
            return ResolveForUserAsync(userId, provider.ToSupportedProvider());
        }
    }
}