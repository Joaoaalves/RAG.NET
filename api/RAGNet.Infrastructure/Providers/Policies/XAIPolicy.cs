using RAGNET.Domain.SeedWork;
using RAGNET.Domain.SharedKernel.Providers;
using RAGNET.Domain.SharedKernel.Providers.Rules;

namespace RAGNET.Infrastructure.Providers.Policies
{
    public partial class XAIPolicy : IProviderPolicy
    {

        public string Name => "xAI";
        public string Prefix => "xai-";
        public string Pattern => "^xai-[0-9A-Za-z_-]{80}$";
        public string Url => "https://console.x.ai/team/default/api-keys";
        public SupportedProvider ProviderType => SupportedProvider.OpenAI;

        public void Validate(string apiKey)
        {
            var regexRule = new ApiKeyMustMatchPatternRule(ProviderType, apiKey, Pattern);

            if (regexRule.IsBroken())
                throw new BusinessRuleValidationException(regexRule);

        }
    }
}
