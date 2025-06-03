using RAGNET.Application.DTOs.ProviderApiKey;
using RAGNET.Domain.Repositories;
using RAGNET.Domain.Services.ApiKey;
using RAGNET.Domain.SharedKernel.Providers;

namespace RAGNET.Application.UseCases.ProviderApiKeyUseCases
{
    public interface IUpdateProviderApiKeyUseCase
    {
        Task<Domain.Entities.ProviderApiKey> ExecuteAsync(UpdateProviderApiKeyDTO dto, Guid providerId, string userId);
    }

    public class UpdateProviderApiKeyUseCase(
        IProviderApiKeyRepository providerApiKeyRepository,
        ICryptoService cryptoService
    ) : IUpdateProviderApiKeyUseCase
    {
        private readonly IProviderApiKeyRepository _providerApiKeyRepository = providerApiKeyRepository;

        private readonly ICryptoService _cryptoService = cryptoService;

        public async Task<Domain.Entities.ProviderApiKey> ExecuteAsync(UpdateProviderApiKeyDTO dto, Guid providerId, string userId)
        {
            try
            {
                var userApiKey = await _providerApiKeyRepository.GetByIdAsync(providerId, userId) ?? throw new Exception("User API key not found");

                var encryptedApiKey = _cryptoService.Encrypt(dto.ApiKey);
                userApiKey.Provider = new Provider(
                    userApiKey.Provider.Id,
                    encryptedApiKey);

                await _providerApiKeyRepository.UpdateAsync(userApiKey, userId);

                return userApiKey;
            }
            catch (Exception ex)
            {
                throw new Exception("Error updating user API key", ex);
            }
        }
    }

}