using Microsoft.AspNetCore.Identity;
using RAGNET.Application.Configuration.Commands;
using RAGNET.Application.Subscriptions.Services;
using RAGNET.Domain.SeedWork;
using RAGNET.Domain.SharedKernel.Subscriptions;
using RAGNET.Domain.Users;
using RAGNET.Domain.Users.Subscriptions;

namespace RAGNET.Application.Subscriptions.Commands.ChangeSubscriptionPlan
{
    public class ChangeSubscriptionPlanCommandHandler(
        UserManager<User> userManager,
        ISubscriptionRepository subscriptionRepository,
        IPaymentGateway paymentGateway,
        IUnitOfWork unitOfWork
    ) : ICommandHandler<ChangeSubscriptionPlanCommand, Unit>
    {
        private readonly UserManager<User> _userManager = userManager;
        private readonly IPaymentGateway _paymentGateway = paymentGateway;
        private readonly ISubscriptionRepository _subscriptionRepository = subscriptionRepository;
        private readonly IUnitOfWork _unitOfWork = unitOfWork;
        public async Task<Unit> Handle(ChangeSubscriptionPlanCommand request, CancellationToken cancellationToken)
        {
            var user = request.User;

            var subscription = await _subscriptionRepository.GetByUserIdAsync(user.Id);

            if (string.IsNullOrEmpty(user.CustomerId) || string.IsNullOrEmpty(subscription?.SubscriptionId))
                throw new ApplicationException("Invalid subscription id.");

            var prorate = subscription.Plan.Value < request.NewPlan;

            await _paymentGateway.ChangeSubscriptionPlanAsync(request.NewPlan, subscription.SubscriptionId, prorate);

            if (!prorate)
            {
                subscription.SchedulePlanChange(SubscriptionPlan.FromType(request.NewPlan));
                await _subscriptionRepository.UpdateAsync(subscription);
            }

            await _unitOfWork.CommitAsync(cancellationToken);
            return Unit.Value;
        }
    }
}