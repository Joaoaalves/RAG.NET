using RAGNET.Application.DTOs.ProviderApiKey;
using RAGNET.Domain.Entities;
using RAGNET.Domain.SharedKernel.Providers;

namespace RAGNET.Application.Mappers
{
    public static class ProviderApiKeyMapper
    {
        public static ProviderApiKey ToProviderApiKey(this CreateProviderApiKeyDTO dto, string userId)
        {
            return new ProviderApiKey
            {
                Id = Guid.NewGuid(),
                UserId = userId,
                Provider = new Provider(dto.Provider, dto.ApiKey),
            };
        }

        public static ProviderApiKeyDTO ToDTO(this ProviderApiKey userApiKey)
        {
            return new ProviderApiKeyDTO
            {
                Id = userApiKey.Id.ToString(),
                ApiKey = userApiKey.Provider.ApiKey.Suffix,
                UserId = userApiKey.UserId,
                Provider = userApiKey.Provider.Type
            };
        }
    }
}