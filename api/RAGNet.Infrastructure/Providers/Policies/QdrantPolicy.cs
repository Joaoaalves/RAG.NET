using RAGNET.Domain.SeedWork;
using RAGNET.Domain.SharedKernel.Providers;
using RAGNET.Domain.SharedKernel.Providers.Rules;

namespace RAGNET.Infrastructure.Providers.Policies
{
    public partial class QdrantPolicy : IProviderPolicy
    {

        public string Name => "QDrant";
        public string Prefix => "";
        public string Pattern => "^[\\w-]*.[\\w-]*.[\\w-]*$";
        public string Url => "https://cloud.qdrant.io/";
        public SupportedProvider ProviderType => SupportedProvider.OpenAI;

        public void Validate(string apiKey)
        {
            var regexRule = new ApiKeyMustMatchPatternRule(ProviderType, apiKey, Pattern);

            if (regexRule.IsBroken())
                throw new BusinessRuleValidationException(regexRule);

        }
    }
}
