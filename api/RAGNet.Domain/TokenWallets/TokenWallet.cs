using RAGNET.Domain.SeedWork;
using RAGNET.Domain.SharedKernel.Tokens;
using RAGNET.Domain.TokenWallets.Rules;
using RAGNET.Domain.TokenWallets.TokenTransactions;

namespace RAGNET.Domain.TokenWallets
{
    public class TokenWallet : Entity, IAggregateRoot, IUserOwned
    {
        private readonly List<TokenTransaction> _transactions = [];
        public TokenWalletId Id { get; private init; } = default!;
        public string UserId { get; set; } = string.Empty;
        public TokenAmount FreeTokens { get; private set; } = default!;
        public TokenAmount PaidTokens { get; private set; } = default!;
        public DateTime LastFreeTokenResetAt { get; private set; }

        public IReadOnlyCollection<TokenTransaction> Transactions => _transactions;

        // EF Core
        private TokenWallet() { }

        private TokenWallet(
            TokenWalletId id,
            string userId,
            TokenAmount freeTokens,
            TokenAmount paidTokens,
            DateTime lastFreeTokenResetAt
        )
        {
            Id = id;
            UserId = userId;
            FreeTokens = freeTokens;
            PaidTokens = paidTokens;
            LastFreeTokenResetAt = lastFreeTokenResetAt;
        }

        public static TokenWallet Create(
            string userId,
            TokenAmount freeTokens,
            TokenAmount paidTokens,
            TokenWalletId? tokenWalletId = null,
            DateTime? lastFreeTokenResetAt = null
        )
        {
            return new TokenWallet(
                tokenWalletId ?? new TokenWalletId(Guid.NewGuid()),
                userId,
                freeTokens,
                paidTokens,
                lastFreeTokenResetAt ?? DateTime.UtcNow
            );
        }

        public void Consume(TokenAmount amount)
        {
            CheckRule(new MustHaveSufficientTokensRule(this, amount));
            if (FreeTokens.Value >= amount.Value)
            {
                FreeTokens = TokenAmount.FromMilitokens(FreeTokens.Value - amount.Value);
            }
            else
            {
                var remaining = amount.Value - FreeTokens.Value;
                FreeTokens = TokenAmount.Zero;
                PaidTokens = TokenAmount.FromMilitokens(PaidTokens.Value - remaining);
            }
        }

        public void AddPaidTokens(TokenAmount amount)
        {
            PaidTokens = TokenAmount.FromMilitokens(PaidTokens.Value + amount.Value);
        }

        public void ResetFreeTokens(TokenAmount monthlyAmount)
        {
            FreeTokens = monthlyAmount;
            LastFreeTokenResetAt = DateTime.UtcNow;
        }
        public bool HasEnough(TokenAmount amount)
        {
            var totalAvailable = FreeTokens.Value + PaidTokens.Value;
            return totalAvailable >= amount.Value;
        }

        public TokenSource GetTokenSourceFor(TokenAmount amount)
        {
            return FreeTokens.Value >= amount.Value
                ? TokenSource.Free
                : TokenSource.Paid;
        }
    }
}