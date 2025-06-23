using RAGNET.Domain.SeedWork;
using RAGNET.Domain.SharedKernel.Providers;

namespace RAGNET.Domain.ProvidersApiKeys
{
    public class ProviderApiKey : Entity, IUserOwned
    {
        public ProviderApiKeyId Id { get; private set; } = default!;
        public Provider Provider { get; set; } = null!;
        public string UserId { get; set; } = String.Empty;

        // EF Core Ctor
        private ProviderApiKey() { }

        private ProviderApiKey(ProviderApiKeyId id, Provider provider, string userId)
        {
            Id = id;
            Provider = provider;
            UserId = userId;
        }

        public static ProviderApiKey Create(string userId, Provider provider, ProviderApiKeyId? id = null)
        {
            return new ProviderApiKey(id ?? new ProviderApiKeyId(), provider, userId);
        }
    }
}