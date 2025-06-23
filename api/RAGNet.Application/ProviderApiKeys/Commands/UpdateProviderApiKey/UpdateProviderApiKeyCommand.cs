using RAGNET.Application.Configuration.Commands;
using RAGNET.Application.ProviderApiKeys.DTOs;
using RAGNET.Domain.ProvidersApiKeys;

namespace RAGNET.Application.ProviderApiKeys.Commands.UpdateProviderApiKey
{
    public class UpdateProviderApiKeyCommand(
        ProviderApiKeyId providerApiKeyId,
        string apiKey
    ) : UserAwareCommand<ProviderApiKeyDTO>
    {
        public ProviderApiKeyId ProviderApiKeyId { get; } = providerApiKeyId;
        public string ApiKey { get; } = apiKey;
    }
}