using RAGNET.Domain.SeedWork;
using RAGNET.Domain.SharedKernel.ApiKeys;

namespace RAGNET.Domain.SharedKernel.Providers
{
    public class Provider : ValueObject
    {
        public SupportedProvider Type { get; }
        public ApiKey ApiKey { get; } = null!;
        private Provider() { }

        public Provider(SupportedProvider type, string apiKey)
        {
            Type = type;
            ApiKey = new ApiKey(apiKey);
        }

        public static Provider CreateFromEncrypted(SupportedProvider type, string encryptedApiKey)
        {
            return new Provider(type, encryptedApiKey);
        }

        public static Provider CreateValidated(SupportedProvider type, string plainApiKey)
        {
            var policy = ProviderPolicyFactory.GetPolicy(type);
            policy.Validate(plainApiKey);

            return new Provider(type, plainApiKey);
        }
    }
}