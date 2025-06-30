using RAGNET.Application.Configuration.Commands;
using RAGNET.Application.Subscriptions.Services;
using RAGNET.Domain.Users;

namespace RAGNET.Application.Subscriptions.Commands.ProcessSuccessfulCancelSubscription
{
    public class ProcessSuccessfulCancelSubscriptionCommandHandler(
        IPaymentGateway paymentGateway,
        IUserRepository userRepository
    ) : ICommandHandler<ProcessSuccessfulCancelSubscriptionCommand, bool>
    {
        private readonly IPaymentGateway _paymentGateway = paymentGateway;
        private readonly IUserRepository _userRepository = userRepository;
        public async Task<bool> Handle(ProcessSuccessfulCancelSubscriptionCommand request, CancellationToken cancellationToken)
        {
            var user = await _userRepository.GetByCustomerIdAsync(request.CustomerId) ?? throw new ApplicationException("User not found.");

            user.Subscription.Cancel();

            await _userRepository.UpdateAsync(user);

            return true;
        }
    }
}