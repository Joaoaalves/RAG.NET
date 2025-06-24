using RAGNET.Domain.SeedWork;

namespace RAGNET.Domain.TokenWallets.Events
{
    public class TokenWalletCreatedEvent : DomainEventBase
    {
        public TokenWalletId TokenWalletId { get; }

        public TokenWalletCreatedEvent(TokenWalletId id)
        {
            Console.WriteLine("Token Wallet created!");
            TokenWalletId = id;
        }
    }
}