using RAGNET.Application.Configuration.Commands;
using RAGNET.Application.Subscriptions.Services;
using RAGNET.Domain.SeedWork;
using RAGNET.Domain.Users;

namespace RAGNET.Application.Subscriptions.Commands.CancelSubscription
{
    public class CancelSubscriptionCommandHandler(
        IPaymentGateway paymentGateway,
        IUserRepository userRepository,
        IUnitOfWork unitOfWork
    ) : ICommandHandler<CancelSubscriptionCommand, bool>
    {
        private readonly IPaymentGateway _paymentGateway = paymentGateway;

        private readonly IUserRepository _userRepository = userRepository;
        private readonly IUnitOfWork _unitOfWork = unitOfWork;
        public async Task<bool> Handle(CancelSubscriptionCommand request, CancellationToken cancellationToken)
        {
            var user = request.User;
            var passwordCheck = await _userRepository.CheckPassowrd(user, request.UserPassword);

            if (passwordCheck && user.Subscription.SubscriptionId is not null)
            {
                var success = await _paymentGateway.CancelSubscriptionAsync(request.User.Subscription.SubscriptionId!);

                if (success)
                {
                    user.Subscription.Cancel();

                    await _userRepository.UpdateAsync(user);
                }

                await _unitOfWork.CommitAsync(cancellationToken);

                return success;
            }

            return false;
        }
    }
}