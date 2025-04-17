using RAGNET.Application.DTOs.UserApiKey;
using RAGNET.Application.Mappers;
using RAGNET.Domain.Enums;
using RAGNET.Domain.Repositories;
using RAGNET.Domain.Services.ApiKey;

namespace RAGNET.Application.UseCases.UserApiKey
{
    public interface ICreateUserApiKeyUseCase
    {
        Task<Domain.Entities.UserApiKey> ExecuteAsync(CreateUserApiKeyDTO dto, string userId);
    }

    public class CreateUserApiKeyUseCase(IUserApiKeyRepository userApiKeyRepository, ICryptoService cryptoService) : ICreateUserApiKeyUseCase
    {
        private readonly IUserApiKeyRepository _userApiKeyRepository = userApiKeyRepository;
        private readonly ICryptoService _cryptoService = cryptoService;

        public async Task<Domain.Entities.UserApiKey> ExecuteAsync(CreateUserApiKeyDTO dto, string userId)
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

            var userApiKey = dto.ToUserApiKey(userId, suffix);
            await _userApiKeyRepository.AddAsync(userApiKey);

            return userApiKey;
        }
    }
}