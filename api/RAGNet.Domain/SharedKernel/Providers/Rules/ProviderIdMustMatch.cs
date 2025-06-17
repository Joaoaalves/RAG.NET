using RAGNET.Domain.SeedWork;

namespace RAGNET.Domain.SharedKernel.Providers.Rules
{
    public class ProviderIdMustMatchRule(SupportedProvider expectedId, SupportedProvider actualId) : IBusinessRule
    {
        private readonly SupportedProvider _expectedId = expectedId;
        private readonly SupportedProvider _actualId = actualId;

        public string Message => $"The provider ID must match the expected ID: {_expectedId}.";

        public bool IsBroken()
        {
            return _expectedId != _actualId;
        }
    }
}