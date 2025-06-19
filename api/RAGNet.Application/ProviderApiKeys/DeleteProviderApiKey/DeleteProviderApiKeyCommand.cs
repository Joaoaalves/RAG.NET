using RAGNET.Application.Configuration.Commands;
using RAGNET.Domain.ProvidersApiKeys;

namespace RAGNET.Application.ProviderApiKeys.DeleteProviderApiKey
{
    public class DeleteProviderApiKeyCommand(
        string userId,
        ProviderApiKeyId providerApiKeyId
    ) : ICommand<bool>
    {
        public Guid Id { get; } = Guid.NewGuid();
        public string UserId { get; } = userId;
        public ProviderApiKeyId ProviderApiKeyId { get; } = providerApiKeyId;
    }
}