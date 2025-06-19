using RAGNET.Application.DTOs.ProviderApiKey;
using RAGNET.Domain.ProvidersApiKeys;
using RAGNET.Domain.SharedKernel.Providers;

namespace RAGNET.Application.Mappers
{
    public static class ProviderApiKeyMapper
    {
        public static ProviderApiKey ToProviderApiKey(this CreateProviderApiKeyDTO dto, string userId, IProviderPolicy providerPolicy)
        {
            return ProviderApiKey.Create(
                userId,
                new Provider(dto.Provider, dto.ApiKey, providerPolicy)
            );
        }

        public static ProviderApiKeyDTO ToDTO(this ProviderApiKey userApiKey)
        {
            var prov = userApiKey.Provider;

            return new ProviderApiKeyDTO
            {
                Id = userApiKey.Id.Value,
                ProviderId = prov.ProviderType,
                Name = prov.Name,
                Pattern = prov.Pattern,
                Prefix = prov.Prefix,
                ApiKey = prov.ApiKey.Suffix,
                Url = prov.Url
            };
        }
    }
}