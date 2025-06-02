using RAGNET.Application.DTOs.ProviderApiKey;
using RAGNET.Domain.Repositories;
using RAGNET.Domain.Services.ApiKey;
using RAGNET.Domain.SharedKernel.ApiKeys;

namespace RAGNET.Application.UseCases.ProviderApiKey
{
    public interface IUpdateProviderApiKeyUseCase
    {
        Task<Domain.Entities.ProviderApiKey> ExecuteAsync(UpdateProviderApiKeyDTO dto, Guid providerId, string userId);
    }

    public class UpdateProviderApiKeyUseCase(
        IProviderApiKeyRepository userApiKeyRepository,
        ICryptoService cryptoService
    ) : IUpdateProviderApiKeyUseCase
    {
        private readonly IProviderApiKeyRepository _userApiKeyRepository = userApiKeyRepository;

        private readonly ICryptoService _cryptoService = cryptoService;

        public async Task<Domain.Entities.ProviderApiKey> ExecuteAsync(UpdateProviderApiKeyDTO dto, Guid providerId, string userId)
        {
            try
            {
                var userApiKey = await _userApiKeyRepository.GetByIdAsync(providerId, userId) ?? throw new Exception("User API key not found");

                var encryptedApiKey = _cryptoService.Encrypt(dto.ApiKey);
                userApiKey.ApiKey = new ApiKey(encryptedApiKey);

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