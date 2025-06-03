using RAGNET.Application.DTOs.ProviderApiKey;
using RAGNET.Application.Mappers;
using RAGNET.Domain.Repositories;
using RAGNET.Domain.SharedKernel.Providers;

namespace RAGNET.Application.UseCases.ProviderApiKeyUseCases
{
    public interface IGetProviderApiKeysUseCase
    {
        Task<List<ProviderApiKeyDTO>> ExecuteAsync(string userId);
    }

    public class GetProviderApiKeysUseCase(IProviderApiKeyRepository providerApiKeyRepository) : IGetProviderApiKeysUseCase
    {
        private readonly IProviderApiKeyRepository _providerApiKeyRepository = providerApiKeyRepository;

        public async Task<List<ProviderApiKeyDTO>> ExecuteAsync(string userId)
        {
            var userApiKeys = await _providerApiKeyRepository.GetByUserIdAsync(userId);

            var result = new List<ProviderApiKeyDTO>();

            foreach (var apiKey in userApiKeys)
            {
                result.Add(apiKey.ToDTO());
            }

            var allProviders = Enum.GetValues<SupportedProvider>();

            foreach (var provider in allProviders)
            {
                bool alreadyExists = userApiKeys.Any(k => k.Provider.Id == provider);
                if (!alreadyExists)
                {
                    var policy = ProviderPolicyFactory.GetPolicy(provider);

                    result.Add(new ProviderApiKeyDTO
                    {
                        ApiKey = string.Empty,
                        ProviderId = provider,
                        Name = policy.Name,
                        Prefix = policy.Prefix,
                        Pattern = policy.Pattern,
                        Url = policy.Url
                    });
                }
            }

            return result;
        }
    }
}
