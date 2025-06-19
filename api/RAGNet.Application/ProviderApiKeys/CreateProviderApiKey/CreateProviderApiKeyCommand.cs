using RAGNET.Application.Configuration.Commands;
using RAGNET.Domain.SharedKernel.Providers;

namespace RAGNET.Application.ProviderApiKeys.CreateProviderApiKey
{
    public class CreateProviderApiKeyCommand(
        string userId,
        SupportedProvider provider,
        string apiKey
    ) : ICommand<ProviderApiKeyDTO>
    {
        public Guid Id { get; } = Guid.NewGuid();
        public string UserId { get; } = userId;
        public SupportedProvider Provider { get; } = provider;
        public string ApiKey { get; } = apiKey;
    }
}