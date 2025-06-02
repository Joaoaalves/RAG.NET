using RAGNET.Application.DTOs.ProviderApiKey;
using RAGNET.Application.Mappers;
using RAGNET.Domain.Repositories;

namespace RAGNET.Application.UseCases.ProviderApiKey
{
    public interface IGetProviderApiKeysUseCase
    {
        Task<List<ProviderApiKeyDTO>> ExecuteAsync(string userId);
    }

    public class GetProviderApiKeysUseCase(IProviderApiKeyRepository userApiKeyRepository) : IGetProviderApiKeysUseCase
    {
        private readonly IProviderApiKeyRepository _userApiKeyRepository = userApiKeyRepository;

        public async Task<List<ProviderApiKeyDTO>> ExecuteAsync(string userId)
        {
            var result = await _userApiKeyRepository.GetByUserIdAsync(userId);
            List<ProviderApiKeyDTO> apiKeys = [];

            foreach (var apiKey in result)
            {
                apiKeys.Add(apiKey.ToDTO());
            }

            return apiKeys;
        }
    }
}