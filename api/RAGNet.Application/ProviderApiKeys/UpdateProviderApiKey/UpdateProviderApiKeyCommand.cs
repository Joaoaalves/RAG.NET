using RAGNET.Application.Configuration.Commands;
using RAGNET.Domain.ProvidersApiKeys;

namespace RAGNET.Application.ProviderApiKeys.UpdateProviderApiKey
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