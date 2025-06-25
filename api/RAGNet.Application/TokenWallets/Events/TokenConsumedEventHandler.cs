using RAGNET.Domain.SeedWork;
using RAGNET.Domain.TokenWallets;
using RAGNET.Domain.TokenWallets.Events;
using RAGNET.Domain.TokenWallets.TokenTransactions;

namespace RAGNET.Application.TokenWallets.Events
{
    public class TokenConsumedNotificationHandler(ITokenWalletRepository tokenWalletRepository, IUnitOfWork unitOfWork) : INotificationHandler<TokenConsumedEvent>
    {
        private readonly ITokenWalletRepository _tokenWalletRepository = tokenWalletRepository;
        private readonly IUnitOfWork _unitOfWork = unitOfWork;

        public async Task Handle(TokenConsumedEvent notification, CancellationToken cancellationToken)
        {
            var transaction = TokenTransaction.Create(
                notification.UserId,
                notification.WorkflowId,
                notification.TokenWalletId,
                notification.Operation,
                notification.ContextInfo,
                notification.Amount,
                notification.Source
            );

            var wallet = await _tokenWalletRepository.GetByUserIdAsync(notification.UserId) ?? throw new Exception("Wallet not found");

            wallet.AddTransaction(transaction);

            await _tokenWalletRepository.UpdateAsync(wallet);
            await _unitOfWork.CommitAsync(cancellationToken);
        }
    }

}