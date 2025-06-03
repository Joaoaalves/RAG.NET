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
            var prov = userApiKey.Provider;
            return new ProviderApiKeyDTO
            {
                Id = userApiKey.Id,
                Name = prov.Name,
                Pattern = prov.Pattern,
                ApiKey = prov.ApiKey.Suffix,
                ProviderId = prov.Id,
                Prefix = prov.Prefix,
                Url = prov.Url
            };
        }
    }
}