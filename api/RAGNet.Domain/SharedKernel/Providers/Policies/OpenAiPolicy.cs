using System.Text.RegularExpressions;
using RAGNET.Domain.SeedWork;
using RAGNET.Domain.SharedKernel.Providers;
using RAGNET.Domain.SharedKernel.Providers.Rules;

namespace RAGNET.Domain.Providers.SharedKernel.Policies
{
    public partial class OpenAiPolicy : IProviderPolicy
    {

        public string Name => "OpenAI";
        public string Prefix => "sk-proj-";
        public string Pattern => "^sk-proj-[A-Za-z0-9_-]{120,200}$";
        public string Url => "https://platform.openai.com/api-keys";
        public static SupportedProvider Id => SupportedProvider.OpenAI;

        SupportedProvider IProviderPolicy.Id => throw new NotImplementedException();

        public void Validate(string apiKey)
        {
            var regexRule = new ApiKeyMustMatchPatternRule(Id, apiKey, Pattern);

            if (regexRule.IsBroken())
                throw new BusinessRuleValidationException(regexRule);

        }
    }
}
