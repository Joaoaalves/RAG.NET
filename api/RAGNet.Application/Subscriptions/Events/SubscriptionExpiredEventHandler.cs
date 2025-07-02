using RAGNET.Domain.SeedWork;
using RAGNET.Domain.Users.Subscriptions;
using RAGNET.Domain.Users.Subscriptions.Events;

namespace RAGNET.Application.Subscriptions.Events
{
    public class SubscriptionExpiredEventHandler(
        ISubscriptionRepository subscriptionRepository,
        IUnitOfWork unitOfWork
    ) : INotificationHandler<SubscriptionExpiredEvent>
    {
        private readonly ISubscriptionRepository _subscriptionRepository = subscriptionRepository;
        private readonly IUnitOfWork _unitOfWork = unitOfWork;
        public async Task Handle(SubscriptionExpiredEvent notification, CancellationToken cancellationToken)
        {
            await _subscriptionRepository.UpdateAsync(notification.Subscription);
            await _unitOfWork.CommitAsync(cancellationToken);

            // Send Email
        }
    }
}