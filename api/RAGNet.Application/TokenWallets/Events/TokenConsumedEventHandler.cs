using RAGNET.Domain.SeedWork;
using RAGNET.Domain.TokenWallets;
using RAGNET.Domain.TokenWallets.Events;
using RAGNET.Domain.TokenWallets.TokenTransactions;

namespace RAGNET.Application.TokenWallets.Events
{
    public class TokenConsumedNotificationHandler(ITokenWalletRepository repo, IUnitOfWork uow) : INotificationHandler<TokenConsumedEvent>
    {
        private readonly ITokenWalletRepository _repo = repo;
        private readonly IUnitOfWork _uow = uow;

        public async Task Handle(TokenConsumedEvent notification, CancellationToken cancellationToken)
        {
            var transaction = TokenTransaction.Create(
                notification.UserId,
                notification.TokenWalletId,
                notification.Operation,
                notification.ContextInfo,
                notification.Amount,
                notification.Source
            );

            var wallet = await _repo.GetByUserIdAsync(notification.UserId) ?? throw new Exception("Wallet not found");

            wallet.AddTransaction(transaction);

            await _repo.UpdateAsync(wallet);
            await _uow.CommitAsync(cancellationToken);
        }
    }

}