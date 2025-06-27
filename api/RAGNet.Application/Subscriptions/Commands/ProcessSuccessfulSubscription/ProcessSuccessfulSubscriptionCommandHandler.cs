using RAGNET.Application.Configuration.Commands;
using RAGNET.Domain.SeedWork;
using RAGNET.Domain.SharedKernel.Subscriptions;
using RAGNET.Domain.Users.Subscriptions;

namespace RAGNET.Application.Subscriptions.Commands.ProcessSuccessfulSubscription
{
    public class ProcessSuccessfulSubscriptionCommandHandler(
        ISubscriptionRepository subscriptionRepository,
        IUnitOfWork unitOfWork
    ) : ICommandHandler<ProcessSuccessfulSubscriptionCommand, Unit>
    {
        private readonly ISubscriptionRepository _subscriptionRepository = subscriptionRepository;
        private readonly IUnitOfWork _unitOfWork = unitOfWork;
        public async Task<Unit> Handle(ProcessSuccessfulSubscriptionCommand request, CancellationToken cancellationToken)
        {
            var subscription = await _subscriptionRepository.GetByUserIdAsync(request.UserId) ?? throw new ApplicationException("Subscription not found");

            var subscriptionPlan = SubscriptionPlan.FromType(request.PlanType);

            subscription.Renew(subscriptionPlan, request.PaymentId);

            await _subscriptionRepository.UpdateAsync(subscription);

            await _unitOfWork.CommitAsync(cancellationToken);

            return Unit.Value;
        }
    }
}