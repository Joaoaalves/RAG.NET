using System.ComponentModel.DataAnnotations.Schema;
using RAGNET.Domain.SeedWork;
using RAGNET.Domain.Users.ApiKeys;

namespace RAGNET.Domain.SharedKernel.Providers
{
    public class Provider : ValueObject
    {
        public SupportedProvider Id { get; }
        public ApiKey ApiKey { get; } = null!;
        [NotMapped]
        public IProviderPolicy? Policy { get; private set; }
        public string Name => Policy?.Name ?? "";
        public string Prefix => Policy?.Prefix ?? "";
        public string Pattern => Policy?.Pattern ?? "";
        public string Url => Policy?.Url ?? "";

        private Provider() { }
        public Provider(SupportedProvider providerId, string apiKeyValue, IProviderPolicy? policy = null, bool validate = true)
        {
            if (policy != null && validate)
            {
                // validate provider ID
                CheckRule(new Rules.ProviderIdMustMatchRule(providerId, policy.Id));

                // validates via policy
                policy.Validate(apiKeyValue);
            }

            Id = providerId;
            ApiKey = new ApiKey(apiKeyValue);
            Policy = policy;
        }

        public static Provider CreateFromPlainText(SupportedProvider providerId, string apiKeyValue, IProviderPolicy policy)
            => new(providerId, apiKeyValue, policy);

        public void InitializePolicy(IProviderPolicy policy)
        {
            Policy = policy;
        }
    }
}