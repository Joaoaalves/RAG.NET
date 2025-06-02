using RAGNET.Application.DTOs.ProviderApiKey;
using RAGNET.Domain.Entities;
using RAGNET.Domain.SharedKernel.ApiKeys;

namespace RAGNET.Application.Mappers
{
    public static class ProviderApiKeyMapper
    {
        public static ProviderApiKey ToProviderApiKey(this CreateProviderApiKeyDTO dto, string userId, string suffix)
        {
            return new ProviderApiKey
            {
                Id = Guid.NewGuid(),
                UserId = userId,
                ApiKey = new ApiKey(dto.ApiKey),
                Provider = dto.Provider
            };
        }

        public static ProviderApiKeyDTO ToDTO(this ProviderApiKey userApiKey)
        {
            return new ProviderApiKeyDTO
            {
                Id = userApiKey.Id.ToString(),
                ApiKey = userApiKey.ApiKey.Suffix,
                UserId = userApiKey.UserId,
                Provider = userApiKey.Provider
            };
        }
    }
}