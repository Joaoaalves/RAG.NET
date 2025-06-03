using System.Text.RegularExpressions;
using RAGNET.Domain.SharedKernel.Providers;
using RAGNET.Domain.SeedWork;

namespace RAGNET.Domain.Providers.SharedKernel.Policies
{
    public partial class VoyagePolicy : IProviderPolicy
    {
        private const string Pattern = "^pa-[A-Za-z0-9-]{30,60}$";

        public static SupportedProvider Type => SupportedProvider.OpenAI;

        SupportedProvider IProviderPolicy.Type => throw new NotImplementedException();

        public void Validate(string apiKey)
        {
            if (!Regex().IsMatch(apiKey))
                throw new BusinessRuleValidationException(
                    new ProviderApiKeyMustMatchPatternRule(Type, Pattern)
                );
        }

        [GeneratedRegex(Pattern)]
        private static partial Regex Regex();
    }
}
