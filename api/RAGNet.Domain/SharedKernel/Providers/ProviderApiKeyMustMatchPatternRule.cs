using RAGNET.Domain.SeedWork;

namespace RAGNET.Domain.SharedKernel.Providers
{
    public class ProviderApiKeyMustMatchPatternRule(SupportedProvider type, string pattern) : IBusinessRule
    {
        public string Message => $"The API key does not match the pattern for {Type}.";

        public SupportedProvider Type { get; } = type;
        public string ExpectedPattern { get; } = pattern;

        public bool IsBroken() => true;
    }
}
