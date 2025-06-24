using RAGNET.Domain.SeedWork;
using RAGNET.Domain.SharedKernel.Tokens.Rules;

namespace RAGNET.Domain.SharedKernel.Tokens
{
    public class TokenAmount : ValueObject
    {
        public long Value { get; }

        public static readonly TokenAmount Zero = new(0);

        public TokenAmount(decimal tokens)
        {
            CheckRule(new TokensMustBeGreaterThanOrEqualToZero((long)tokens / 1000));
            Value = (long)(tokens * 1000m);
        }

        private TokenAmount(long militokens)
        {
            CheckRule(new TokensMustBeGreaterThanOrEqualToZero(militokens));

            Value = militokens;
        }

        public static TokenAmount FromMilitokens(long militokens) => new(militokens);

        public decimal ToDecimal() => Value / 1000m;

        public static TokenAmount operator +(TokenAmount a, TokenAmount b)
            => FromMilitokens(a.Value + b.Value);

        public static TokenAmount operator -(TokenAmount a, TokenAmount b)
            => FromMilitokens(a.Value - b.Value);

        public static bool operator <(TokenAmount a, TokenAmount b)
            => a.Value < b.Value;

        public static bool operator >(TokenAmount a, TokenAmount b)
            => a.Value > b.Value;

        public static bool operator >=(TokenAmount a, TokenAmount b)
            => a.Value >= b.Value;

        public static bool operator <=(TokenAmount a, TokenAmount b)
            => a.Value <= b.Value;
    }
}
