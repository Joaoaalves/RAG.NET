using RAGNET.Domain.SeedWork;
using RAGNET.Domain.TokenWallets;
using RAGNET.Domain.Users.Subscriptions.Events;

namespace RAGNET.Application.Subscriptions.Events
{
    public class SubscriptionCreatedEventHandler(
        ITokenWalletRepository tokenWalletRepository,
        IUnitOfWork unitOfWork
    ) : INotificationHandler<SubscriptionCreatedEvent>
    {
        private readonly ITokenWalletRepository _tokenWalletRepository = tokenWalletRepository;
        private readonly IUnitOfWork _unitOfWork = unitOfWork;

        public async Task Handle(SubscriptionCreatedEvent notification, CancellationToken cancellationToken)
        {
            var wallet = await _tokenWalletRepository.GetByUserIdAsync(notification.Subscription.UserId) ?? throw new ApplicationException("Wallet not found");

            wallet.AddPaidTokens(
                notification.Subscription.Plan.GetTokenAllowance(notification.BillingPeriod),
                notification.Subscription.PaymentId!
            );

            await _tokenWalletRepository.UpdateAsync(wallet);

            await _unitOfWork.CommitAsync(cancellationToken);
        }
    }
}