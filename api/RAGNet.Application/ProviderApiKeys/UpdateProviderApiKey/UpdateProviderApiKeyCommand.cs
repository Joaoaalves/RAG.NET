using RAGNET.Application.Configuration.Commands;
using RAGNET.Domain.ProvidersApiKeys;

namespace RAGNET.Application.ProviderApiKeys.UpdateProviderApiKey
{
    public class UpdateProviderApiKeyCommand(
        string userId,
        ProviderApiKeyId providerApiKeyId,
        string apiKey
    ) : ICommand<ProviderApiKeyDTO>
    {
        public Guid Id { get; } = Guid.NewGuid();
        public string UserId { get; } = userId;
        public ProviderApiKeyId ProviderApiKeyId { get; } = providerApiKeyId;
        public string ApiKey { get; } = apiKey;
    }
}