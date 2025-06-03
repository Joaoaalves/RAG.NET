using System.Text.RegularExpressions;
using RAGNET.Domain.SeedWork;
using RAGNET.Domain.SharedKernel.Providers;

namespace RAGNET.Domain.Providers.SharedKernel.Policies
{
    public partial class AnthropicPolicy : IProviderPolicy
    {
        private const string Pattern = "^sk-ant-[a-z0-9-]+-[A-Za-z0-9_-]{80,140}$";

        public static SupportedProvider Type => SupportedProvider.Anthropic;

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
