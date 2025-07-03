using RAGNET.Domain.SeedWork;
using RAGNET.Domain.SharedKernel.Providers;
using RAGNET.Domain.SharedKernel.Providers.Rules;

namespace RAGNET.Infrastructure.Providers.Policies
{
    public partial class MistralPolicy : IProviderPolicy
    {
        public string Name => "Mistral";
        public string Prefix => "";
        public string Pattern => "^[A-Za-z0-9]{32}$";
        public string Url => "https://admin.mistral.ai/organization/api-keys";
        public SupportedProvider ProviderType => SupportedProvider.Anthropic;

        public void Validate(string apiKey)
        {
            var regexRule = new ApiKeyMustMatchPatternRule(ProviderType, apiKey, Pattern);

            if (regexRule.IsBroken())
                throw new BusinessRuleValidationException(regexRule);

        }
    }
}
