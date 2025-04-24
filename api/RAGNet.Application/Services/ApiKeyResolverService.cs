using RAGNET.Application.Mappers;
using RAGNET.Domain.Enums;
using RAGNET.Domain.Repositories;
using RAGNET.Domain.Services.ApiKey;

namespace RAGNET.Application.Services
{
    public class ApiKeyResolverService(IUserApiKeyRepository userApiKeyRepository, ICryptoService cryptoService) : IApiKeyResolverService
    {
        private readonly IUserApiKeyRepository _userApiKeyRepository = userApiKeyRepository;
        private readonly ICryptoService _cryptoService = cryptoService;

        public async Task<string> ResolveForUserAsync(string userId, SupportedProvider provider)
        {
            var userApiKey = await _userApiKeyRepository.GetByUserIdAndProviderAsync(userId, provider);

            if (userApiKey == null)
                return String.Empty;

            // Decrypt
            var apiKey = _cryptoService.Decrypt(userApiKey.ApiKey);

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