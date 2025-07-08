using RAGNET.Domain.SeedWork;
using RAGNET.Domain.SharedKernel.Providers;
using RAGNET.Domain.SharedKernel.Providers.Rules;

namespace RAGNET.Infrastructure.Providers.Policies
{
    public partial class AnthropicPolicy : IProviderPolicy
    {
        public string Name => "Anthropic";
        public string Prefix => "sk-ant-";
        public string Pattern => "^sk-ant-[a-z0-9-]+-[A-Za-z0-9_-]{80,140}$";
        public string Url => "https://console.anthropic.com/settings/keys";
        public SupportedProvider ProviderType => SupportedProvider.ANTHROPIC;

        public void Validate(string apiKey)
        {
            var regexRule = new ApiKeyMustMatchPatternRule(apiKey, Pattern);

            if (regexRule.IsBroken())
                throw new BusinessRuleValidationException(regexRule);

        }
    }
}
