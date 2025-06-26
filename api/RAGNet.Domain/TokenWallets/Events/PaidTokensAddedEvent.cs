using RAGNET.Domain.SeedWork;
using RAGNET.Domain.SharedKernel.Tokens;

namespace RAGNET.Domain.TokenWallets.Events
{
    public class PaidTokensAddedEvent : DomainEventBase
    {
        public TokenWalletId TokenWalletId { get; }
        public string UserId { get; } = string.Empty;
        public TokenAmount TokenAmount { get; }
        public PaidTokensAddedEvent(TokenWalletId tokenWalletId, string userId, TokenAmount tokenAmount)
        {
            TokenWalletId = tokenWalletId;
            UserId = userId;
            TokenAmount = tokenAmount;

            Console.WriteLine($"{TokenAmount.Value} added to wallet with id: {TokenWalletId} - user: {UserId}");
        }
    }
}