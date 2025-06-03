using System.Text.RegularExpressions;
using RAGNET.Domain.SeedWork;

namespace RAGNET.Domain.SharedKernel.Providers.Rules
{
    public class ApiKeyMustMatchPatternRule(SupportedProvider type, string apiKey, string pattern) : IBusinessRule
    {
        public string Message => $"The API key does not match the pattern for {Type}.";

        public SupportedProvider Type { get; } = type;
        public string ExpectedPattern { get; } = pattern;
        public string ApiKey { get; } = apiKey;
        public bool IsBroken()
        {
            return !Regex.IsMatch(ApiKey, ExpectedPattern);
        }

    }
}
