using RAGNET.Domain.SeedWork;
using RAGNET.Domain.SharedKernel.Providers;
using RAGNET.Domain.SharedKernel.Providers.Rules;

namespace RAGNET.Infrastructure.Providers.Policies
{
    public partial class VoyagePolicy : IProviderPolicy
    {

        public string Name => "Voyage";
        public string Prefix => "pa-";
        public string Pattern => "^pa-[A-Za-z0-9-_-]{30,60}$";
        public string Url => "https://voyage.ai/account/settings";
        public static SupportedProvider Id => SupportedProvider.OpenAI;

        SupportedProvider IProviderPolicy.ProviderType => throw new NotImplementedException();

        public void Validate(string apiKey)
        {
            var regexRule = new ApiKeyMustMatchPatternRule(Id, apiKey, Pattern);

            if (regexRule.IsBroken())
                throw new BusinessRuleValidationException(regexRule);
        }
    }
}
