using RAGNET.Application.Configuration.Commands;
using RAGNET.Domain.SharedKernel.Providers;

namespace RAGNET.Application.ProviderApiKeys.CreateProviderApiKey
{
    public class CreateProviderApiKeyCommand(
        SupportedProvider provider,
        string apiKey
    ) : UserAwareCommand<ProviderApiKeyDTO>
    {
        public SupportedProvider Provider { get; } = provider;
        public string ApiKey { get; } = apiKey;
    }
}