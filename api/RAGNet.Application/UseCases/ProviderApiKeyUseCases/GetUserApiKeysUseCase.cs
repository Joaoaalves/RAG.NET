using RAGNET.Application.DTOs.ProviderApiKey;
using RAGNET.Application.Mappers;

using RAGNET.Domain.ProvidersApiKeys;
using RAGNET.Domain.SharedKernel.Providers;


namespace RAGNET.Application.UseCases.ProviderApiKeyUseCases
{
    public interface IGetProviderApiKeysUseCase
    {
        Task<List<ProviderApiKeyDTO>> ExecuteAsync(string userId);
    }

    public class GetProviderApiKeysUseCase(IProviderApiKeyRepository providerApiKeyRepository, IProviderPolicyFactory providerPolicyFactory) : IGetProviderApiKeysUseCase
    {
        private readonly IProviderApiKeyRepository _providerApiKeyRepository = providerApiKeyRepository;
        private readonly IProviderPolicyFactory _providerPolicyFactory = providerPolicyFactory;

        public async Task<List<ProviderApiKeyDTO>> ExecuteAsync(string userId)
        {
            var userApiKeys = await _providerApiKeyRepository.GetByUserIdAsync(userId);
            var result = new List<ProviderApiKeyDTO>();

            foreach (var apiKey in userApiKeys)
            {
                var provider = apiKey.Provider.ProviderType;
                var policy = _providerPolicyFactory.GetPolicy(provider);
                apiKey.Provider.InitializePolicy(policy);
                result.Add(apiKey.ToDTO());
            }

            // Add missing providers
            // Get all supported providers
            var allProviders = Enum.GetValues<SupportedProvider>();

            foreach (var provider in allProviders)
            {
                bool alreadyExists = userApiKeys.Any(k => k.Provider.ProviderType == provider);
                if (!alreadyExists)
                {
                    var policy = _providerPolicyFactory.GetPolicy(provider);
                    var dto = new ProviderApiKeyDTO
                    {
                        ApiKey = string.Empty,
                        ProviderId = provider,
                        Name = policy.Name,
                        Prefix = policy.Prefix,
                        Pattern = policy.Pattern,
                        Url = policy.Url
                    };


                    result.Add(dto);
                }
            }

            return result;
        }
    }
}
