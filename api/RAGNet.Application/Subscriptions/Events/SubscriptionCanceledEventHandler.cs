using RAGNET.Domain.SeedWork;
using RAGNET.Domain.Users.Subscriptions;
using RAGNET.Domain.Users.Subscriptions.Events;

namespace RAGNET.Application.Subscriptions.Events
{
    public class SubscriptionCanceledEventHandler(
        ISubscriptionRepository subscriptionRepository,
        IUnitOfWork unitOfWork
    ) : INotificationHandler<SubscriptionCanceledEvent>
    {
        private readonly ISubscriptionRepository _subscriptionRepository = subscriptionRepository;
        private readonly IUnitOfWork _unitOfWork = unitOfWork;
        public async Task Handle(SubscriptionCanceledEvent notification, CancellationToken cancellationToken)
        {
            await _subscriptionRepository.UpdateAsync(notification.Subscription);
            await _unitOfWork.CommitAsync(cancellationToken);

            // Send Cancelation E-mail
            return;
        }
    }
}