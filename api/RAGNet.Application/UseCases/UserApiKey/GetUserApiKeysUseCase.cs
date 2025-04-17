using RAGNET.Application.DTOs.UserApiKey;
using RAGNET.Application.Mappers;
using RAGNET.Domain.Repositories;

namespace RAGNET.Application.UseCases.UserApiKey
{
    public interface IGetUserApiKeysUseCase
    {
        Task<List<UserApiKeyDTO>> ExecuteAsync(string userId);
    }

    public class GetUserApiKeysUseCase(IUserApiKeyRepository userApiKeyRepository) : IGetUserApiKeysUseCase
    {
        private readonly IUserApiKeyRepository _userApiKeyRepository = userApiKeyRepository;

        public async Task<List<UserApiKeyDTO>> ExecuteAsync(string userId)
        {
            var result = await _userApiKeyRepository.GetByUserIdAsync(userId);
            List<UserApiKeyDTO> apiKeys = [];

            foreach (var apiKey in result)
            {
                apiKeys.Add(apiKey.ToDTO());
            }

            return apiKeys;
        }
    }
}