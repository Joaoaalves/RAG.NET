using RAGNET.Application.DTOs.ProviderApiKey;
using RAGNET.Application.Mappers;
using RAGNET.Domain.Enums;
using RAGNET.Domain.Repositories;
using RAGNET.Domain.Services.ApiKey;

namespace RAGNET.Application.UseCases.ProviderApiKey
{
    public interface ICreateProviderApiKeyUseCase
    {
        Task<Domain.Entities.ProviderApiKey> ExecuteAsync(CreateProviderApiKeyDTO dto, string userId);
    }

    public class CreateProviderApiKeyUseCase(IProviderApiKeyRepository userApiKeyRepository, ICryptoService cryptoService) : ICreateProviderApiKeyUseCase
    {
        private readonly IProviderApiKeyRepository _userApiKeyRepository = userApiKeyRepository;
        private readonly ICryptoService _cryptoService = cryptoService;

        public async Task<Domain.Entities.ProviderApiKey> ExecuteAsync(CreateProviderApiKeyDTO dto, string userId)
        {
            if (string.IsNullOrEmpty(dto.ApiKey))
                throw new ArgumentException("API key cannot be null or empty.", nameof(dto.ApiKey));

            if (!Enum.IsDefined(dto.Provider))
                throw new ArgumentException($"Invalid provider: {dto.Provider}", nameof(dto.Provider));

            if (await _userApiKeyRepository.ExistsAsync(dto.Provider, userId))
                throw new InvalidOperationException("API key already exists for this user.");

            var encryptedApiKey = _cryptoService.Encrypt(dto.ApiKey);
            var suffix = dto.ApiKey.Substring(dto.ApiKey.Length - 4, 4);
            dto.ApiKey = encryptedApiKey;

            var userApiKey = dto.ToProviderApiKey(userId, suffix);
            await _userApiKeyRepository.AddAsync(userApiKey);

            return userApiKey;
        }
    }
}