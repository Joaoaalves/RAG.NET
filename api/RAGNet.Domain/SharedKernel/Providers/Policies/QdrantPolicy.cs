using System.Text.RegularExpressions;
using RAGNET.Domain.SharedKernel.Providers;
using RAGNET.Domain.SeedWork;
using RAGNET.Domain.SharedKernel.Providers.Rules;

namespace RAGNET.Domain.Providers.SharedKernel.Policies
{
    public partial class QdrantPolicy : IProviderPolicy
    {

        public string Name => "QDrant";
        public string Prefix => "";
        public string Pattern => "^[\\w-]*.[\\w-]*.[\\w-]*$";
        public string Url => "https://cloud.qdrant.io/";
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
