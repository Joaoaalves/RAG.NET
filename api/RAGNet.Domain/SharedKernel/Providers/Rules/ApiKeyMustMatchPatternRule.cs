using System.Text.RegularExpressions;
using RAGNET.Domain.SeedWork;

namespace RAGNET.Domain.SharedKernel.Providers.Rules
{
    public class ApiKeyMustMatchPatternRule(string apiKey, string pattern) : IBusinessRule
    {
        public string Message => "The API key does not match the pattern.";
        public string ExpectedPattern { get; } = pattern;
        public string ApiKey { get; } = apiKey;
        public bool IsBroken()
        {
            return !Regex.IsMatch(ApiKey, ExpectedPattern);
        }

    }
}
