using RAGNET.Application.ProviderApiKeys.Commands.CreateProviderApiKey;
using RAGNET.Application.ProviderApiKeys.DTOs;
using RAGNET.Domain.ProvidersApiKeys;
using RAGNET.Domain.SharedKernel.Providers;

namespace RAGNET.Application.Infrastructure.Providers.Embedding
{
    public static class ProviderApiKeyMapper
    {
        public static ProviderApiKey ToProviderApiKey(this CreateProviderApiKeyRequest request, string userId, IProviderPolicy providerPolicy)
        {
            return ProviderApiKey.Create(
                userId,
                new Provider(request.Provider, request.ApiKey, providerPolicy)
            );
        }

        public static ProviderApiKeyDTO ToDTO(this ProviderApiKey userApiKey)
        {
            var prov = userApiKey.Provider;

            return new ProviderApiKeyDTO
            {
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