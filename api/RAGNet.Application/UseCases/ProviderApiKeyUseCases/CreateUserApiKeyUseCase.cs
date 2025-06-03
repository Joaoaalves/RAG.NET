using RAGNET.Application.DTOs.ProviderApiKey;
using RAGNET.Application.Mappers;
using RAGNET.Domain.Entities;
using RAGNET.Domain.Repositories;
using RAGNET.Domain.Services.ApiKey;
using RAGNET.Domain.SharedKernel.Providers;

namespace RAGNET.Application.UseCases.ProviderApiKeyUseCases
{
    public interface ICreateProviderApiKeyUseCase
    {
        Task<Domain.Entities.ProviderApiKey> ExecuteAsync(CreateProviderApiKeyDTO dto, string userId);
    }

    public class CreateProviderApiKeyUseCase(IProviderApiKeyRepository providerApiKeyRepository, ICryptoService cryptoService) : ICreateProviderApiKeyUseCase
    {
        private readonly IProviderApiKeyRepository _providerApiKeyRepository = providerApiKeyRepository;
        private readonly ICryptoService _cryptoService = cryptoService;

        public async Task<Domain.Entities.ProviderApiKey> ExecuteAsync(CreateProviderApiKeyDTO dto, string userId)
        {

            if (await _providerApiKeyRepository.ExistsAsync(dto.Provider, userId))
                throw new InvalidOperationException("API key already exists for this user.");

            Console.WriteLine(dto.ApiKey);
            Console.WriteLine(dto.Provider);

            Provider.CreateValidated(dto.Provider, dto.ApiKey); // Just validate

            Console.WriteLine("Created with plain");

            // Hash
            var encryptedApiKey = _cryptoService.Encrypt(dto.ApiKey);

            // Create encrypted provider
            var provider = Provider.CreateFromEncrypted(dto.Provider, encryptedApiKey);
            Console.WriteLine("Created encrypted");
            var providerApiKey = new ProviderApiKey
            {
                Id = Guid.NewGuid(),
                UserId = userId,
                Provider = provider
            };

            await _providerApiKeyRepository.AddAsync(providerApiKey);

            return providerApiKey;
        }
    }
}