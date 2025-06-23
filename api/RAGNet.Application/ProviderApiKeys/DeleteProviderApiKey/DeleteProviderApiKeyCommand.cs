using RAGNET.Application.Configuration.Commands;
using RAGNET.Domain.ProvidersApiKeys;

namespace RAGNET.Application.ProviderApiKeys.DeleteProviderApiKey
{
    public class DeleteProviderApiKeyCommand(
        ProviderApiKeyId providerApiKeyId
    ) : UserAwareCommand<bool>
    {
        public ProviderApiKeyId ProviderApiKeyId { get; } = providerApiKeyId;
    }
}