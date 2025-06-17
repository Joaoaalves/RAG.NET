using RAGNET.Domain.SeedWork;
using RAGNET.Domain.SharedKernel.Providers;

namespace RAGNET.Domain.ProvidersApiKeys
{
    public class ProviderApiKey : Entity, IUserOwned
    {
        public Guid Id { get; set; }
        public Provider Provider { get; set; } = null!;
        public string UserId { get; set; } = String.Empty;

        // EF Core Ctor
        private ProviderApiKey() { }

        private ProviderApiKey(Guid id, Provider provider, string userId)
        {
            Id = id;
            Provider = provider;
            UserId = userId;
        }

        public static ProviderApiKey Create(Guid id, string userId, Provider provider)
        {
            return new ProviderApiKey(id, provider, userId);
        }
    }
}