using RAGNET.Application.ApiKeys;
using RAGNET.Application.DTOs.ProviderApiKey;
using RAGNET.Application.Mappers;

using RAGNET.Domain.ProvidersApiKeys;
using RAGNET.Domain.SeedWork;
using RAGNET.Domain.SharedKernel.Providers;


namespace RAGNET.Application.UseCases.ProviderApiKeyUseCases
{
    public interface IUpdateProviderApiKeyUseCase
    {
        Task<ProviderApiKeyDTO> ExecuteAsync(UpdateProviderApiKeyDTO dto, ProviderApiKeyId providerId, string userId);
    }

    public class UpdateProviderApiKeyUseCase(
        IProviderApiKeyRepository providerApiKeyRepository,
        ICryptoService cryptoService,
        IProviderPolicyFactory providerPolicyFactory,
        IUnitOfWork unitOfWork
    ) : IUpdateProviderApiKeyUseCase
    {
        private readonly IProviderApiKeyRepository _providerApiKeyRepository = providerApiKeyRepository;

        private readonly ICryptoService _cryptoService = cryptoService;
        private readonly IProviderPolicyFactory _providerPolicyFactory = providerPolicyFactory;
        private readonly IUnitOfWork _unitOfWork = unitOfWork;

        public async Task<ProviderApiKeyDTO> ExecuteAsync(UpdateProviderApiKeyDTO dto, ProviderApiKeyId providerId, string userId)
        {
            try
            {
                var userApiKey = await _providerApiKeyRepository.GetByIdAsync(providerId, userId) ?? throw new Exception("User API key not found");

                var encryptedApiKey = _cryptoService.Encrypt(dto.ApiKey);
                var policy = _providerPolicyFactory.GetPolicy(userApiKey.Provider.ProviderType);

                policy.Validate(dto.ApiKey);

                userApiKey.Provider = new Provider(
                    userApiKey.Provider.ProviderType,
                    encryptedApiKey,
                    policy,
                    false
                );

                await _providerApiKeyRepository.UpdateAsync(userApiKey, userId);
                await _unitOfWork.CommitAsync();
                return userApiKey.ToDTO();
            }
            catch (Exception ex)
            {
                await _unitOfWork.RevertAsync();
                throw new Exception("Error updating user API key", ex);
            }
        }
    }

}