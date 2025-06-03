using RAGNET.Application.DTOs.ProviderApiKey;
using RAGNET.Application.Mappers;
using RAGNET.Domain.Repositories;

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
            var result = await _providerApiKeyRepository.GetByUserIdAsync(userId);
            List<ProviderApiKeyDTO> apiKeys = [];

            foreach (var apiKey in result)
            {
                apiKeys.Add(apiKey.ToDTO());
            }

            return apiKeys;
        }
    }
}