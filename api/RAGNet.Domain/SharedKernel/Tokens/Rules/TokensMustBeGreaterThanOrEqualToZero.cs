using RAGNET.Domain.SeedWork;

namespace RAGNET.Domain.SharedKernel.Tokens.Rules
{
    public partial class TokensMustBeGreaterThanOrEqualToZero(long value) : IBusinessRule
    {
        public string Message => "Tokens Must be greater than or equal to Zero.";

        public bool IsBroken() => value < 0;
    }
}