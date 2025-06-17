using RAGNET.Domain.ProvidersApiKeys;
using RAGNET.Domain.SeedWork;
using RAGNET.Domain.SharedKernel.Providers;

using RAGNET.Application.ApiKeys;
using RAGNET.Application.DTOs.ProviderApiKey;
using RAGNET.Application.Mappers;

namespace RAGNET.Application.UseCases.ProviderApiKeyUseCases
{
    public interface ICreateProviderApiKeyUseCase
    {
        Task<ProviderApiKeyDTO> ExecuteAsync(CreateProviderApiKeyDTO dto, string userId);
    }

    public class CreateProviderApiKeyUseCase(IProviderApiKeyRepository providerApiKeyRepository, ICryptoService cryptoService, IUnitOfWork unitOfWork, IProviderPolicyFactory providerPolicyFactory) : ICreateProviderApiKeyUseCase
    {
        private readonly IProviderApiKeyRepository _providerApiKeyRepository = providerApiKeyRepository;
        private readonly ICryptoService _cryptoService = cryptoService;
        private readonly IProviderPolicyFactory _providerPolicyFactory = providerPolicyFactory;
        private readonly IUnitOfWork _unitOfWork = unitOfWork;

        public async Task<ProviderApiKeyDTO> ExecuteAsync(CreateProviderApiKeyDTO dto, string userId)
        {
            try
            {
                // Validate ApiKey
                var policy = _providerPolicyFactory.GetPolicy(dto.Provider);
                policy.Validate(dto.ApiKey);

                // Hash
                var encryptedApiKey = _cryptoService.Encrypt(dto.ApiKey);

                // Create encrypted provider
                var provider = new Provider(providerId: dto.Provider, apiKeyValue: encryptedApiKey, validate: false);

                var providerApiKey = ProviderApiKey.Create(
                    id: Guid.NewGuid(),
                    userId,
                    provider
                );

                await _providerApiKeyRepository.AddAsync(providerApiKey);
                await _unitOfWork.CommitAsync();
                return providerApiKey.ToDTO();
            }
            catch (Exception)
            {
                await _unitOfWork.RevertAsync();
                throw;
            }
        }
    }
}