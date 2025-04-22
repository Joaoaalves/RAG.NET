using RAGNET.Application.DTOs.UserApiKey;
using RAGNET.Domain.Repositories;
using RAGNET.Domain.Services.ApiKey;

namespace RAGNET.Application.UseCases.UserApiKey
{
    public interface IUpdateUserApiKeyUseCase
    {
        Task<Domain.Entities.UserApiKey> ExecuteAsync(UpdateUserApiKeyDTO dto, Guid providerId, string userId);
    }

    public class UpdateUserApiKeyUseCase(
        IUserApiKeyRepository userApiKeyRepository,
        ICryptoService cryptoService
    ) : IUpdateUserApiKeyUseCase
    {
        private readonly IUserApiKeyRepository _userApiKeyRepository = userApiKeyRepository;

        private readonly ICryptoService _cryptoService = cryptoService;

        public async Task<Domain.Entities.UserApiKey> ExecuteAsync(UpdateUserApiKeyDTO dto, Guid providerId, string userId)
        {
            try
            {
                var userApiKey = await _userApiKeyRepository.GetByIdAsync(providerId, userId) ?? throw new Exception("User API key not found");

                var encryptedApiKey = _cryptoService.Encrypt(dto.ApiKey);
                userApiKey.ApiKey = encryptedApiKey;
                userApiKey.KeySuffix = dto.ApiKey.Substring(dto.ApiKey.Length - 4, 4);

                await _userApiKeyRepository.UpdateAsync(userApiKey, userId);

                return userApiKey;
            }
            catch (Exception ex)
            {
                throw new Exception("Error updating user API key", ex);
            }
        }
    }

}