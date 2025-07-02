using RAGNET.Domain.SeedWork;
using RAGNET.Domain.SharedKernel.Providers;
using RAGNET.Domain.SharedKernel.Providers.Rules;

namespace RAGNET.Infrastructure.Providers.Policies
{
    public partial class GeminiPolicy : IProviderPolicy
    {

        public string Name => "Gemini";
        public string Prefix => "AIza";
        public string Pattern => "^AIza[0-9A-Za-z_-]{30,50}$";
        public string Url => "https://aistudio.google.com/app/apikey";
        public SupportedProvider ProviderType => SupportedProvider.OpenAI;

        public void Validate(string apiKey)
        {
            var regexRule = new ApiKeyMustMatchPatternRule(ProviderType, apiKey, Pattern);

            if (regexRule.IsBroken())
                throw new BusinessRuleValidationException(regexRule);

        }
    }
}
