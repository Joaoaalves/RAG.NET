using RAGNET.Application.Mappers;
using RAGNET.Domain.Repositories;
using RAGNET.Domain.Services.ApiKey;
using RAGNET.Domain.SharedKernel.Providers;

namespace RAGNET.Application.Services
{
    public class ApiKeyResolverService(IProviderApiKeyRepository providerApiKeyRepository, ICryptoService cryptoService) : IApiKeyResolverService
    {
        private readonly IProviderApiKeyRepository _providerApiKeyRepository = providerApiKeyRepository;
        private readonly ICryptoService _cryptoService = cryptoService;

        public async Task<string> ResolveForUserAsync(string userId, SupportedProvider provider)
        {
            var userApiKey = await _providerApiKeyRepository.GetByUserIdAndProviderAsync(userId, provider);

            if (userApiKey == null)
                return String.Empty;

            // Decrypt
            var apiKey = _cryptoService.Decrypt(userApiKey.Provider.ApiKey.Value);

            return apiKey;
        }

        public Task<string> ResolveForUserAsync(string userId, ConversationProviderEnum provider)
        {
            return ResolveForUserAsync(userId, provider.ToSupportedProvider());
        }

        public Task<string> ResolveForUserAsync(string userId, EmbeddingProviderEnum provider)
        {
            return ResolveForUserAsync(userId, provider.ToSupportedProvider());
        }
    }
}