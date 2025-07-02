using RAGNET.Application.Configuration.Commands;
using RAGNET.Domain.SeedWork;
using RAGNET.Domain.SharedKernel.Subscriptions;
using RAGNET.Domain.Users;
using RAGNET.Domain.Users.Subscriptions;

namespace RAGNET.Application.Subscriptions.Commands.ProcessSuccessfulSubscription
{
    public class ProcessSuccessfulSubscriptionCommandHandler(
        IUserRepository userRepository,
        ISubscriptionRepository subscriptionRepository,
        IUnitOfWork unitOfWork
    ) : ICommandHandler<ProcessSuccessfulSubscriptionCommand, Unit>
    {
        private readonly IUserRepository _userRepository = userRepository;
        private readonly ISubscriptionRepository _subscriptionRepository = subscriptionRepository;
        private readonly IUnitOfWork _unitOfWork = unitOfWork;
        public async Task<Unit> Handle(ProcessSuccessfulSubscriptionCommand request, CancellationToken cancellationToken)
        {
            var user = await _userRepository.GetByCustomerIdAsync(request.CustomerId) ?? throw new ApplicationException("User not found!");
            var subscription = await _subscriptionRepository.GetByUserIdAsync(user.Id) ?? throw new ApplicationException("Subscription not found");

            // If the subscription is active and is the same plan, just renew
            // Else, just add a new subscription
            if (subscription.Status == SubscriptionStatus.Active && subscription.Plan.Value == request.PlanType)
            {
                subscription.Renew(request.RenewedAt, request.ExpiresAt, request.BillingPeriod);
            }
            else
            {
                var subscriptionPlan = SubscriptionPlan.FromType(request.PlanType);

                subscription.AddSubscription(
                    subscriptionPlan,
                    request.BillingPeriod,
                    request.RenewedAt,
                    request.ExpiresAt,
                    request.PaymentId,
                    request.SubscriptionId
                );
            }


            await _subscriptionRepository.UpdateAsync(subscription);

            await _unitOfWork.CommitAsync(cancellationToken);

            return Unit.Value;
        }
    }
}