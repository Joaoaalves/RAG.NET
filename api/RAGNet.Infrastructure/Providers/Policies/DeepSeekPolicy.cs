using RAGNET.Domain.SeedWork;
using RAGNET.Domain.SharedKernel.Providers;
using RAGNET.Domain.SharedKernel.Providers.Rules;

namespace RAGNET.Infrastructure.Providers.Policies
{
    public partial class DeepSeekPolicy : IProviderPolicy
    {

        public string Name => "DeepSeek";
        public string Prefix => "sk-";
        public string Pattern => "^sk-[A-Za-z0-9_-]{32}$";
        public string Url => "https://platform.deepseek.com/api_keys";
        public SupportedProvider ProviderType => SupportedProvider.OPENAI;

        public void Validate(string apiKey)
        {
            var regexRule = new ApiKeyMustMatchPatternRule(apiKey, Pattern);

            if (regexRule.IsBroken())
                throw new BusinessRuleValidationException(regexRule);

        }
    }
}