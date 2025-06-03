using RAGNET.Domain.SeedWork;
using RAGNET.Domain.SharedKernel.ApiKeys;

namespace RAGNET.Domain.SharedKernel.Providers
{
    public class Provider : ValueObject
    {
        public SupportedProvider Id { get; }
        public ApiKey ApiKey { get; } = null!;
        public IProviderPolicy Policy { get; } = null!;
        public string Name => Policy.Name;
        public string Prefix => Policy.Prefix;
        public string Pattern => Policy.Pattern;
        public string Url => Policy.Url;
        private Provider() { }

        public Provider(SupportedProvider provider, string apiKey)
        {
            Id = provider;
            ApiKey = new ApiKey(apiKey);
            Policy = ProviderPolicyFactory.GetPolicy(provider);
        }

        public static Provider CreateFromEncrypted(SupportedProvider provider, string encryptedApiKey)
        {
            return new Provider(provider, encryptedApiKey);
        }

        public static Provider CreateValidated(SupportedProvider provider, string plainApiKey)
        {
            var policy = ProviderPolicyFactory.GetPolicy(provider);
            policy.Validate(plainApiKey);

            return new Provider(provider, plainApiKey);
        }
    }
}