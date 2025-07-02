using Microsoft.AspNetCore.Identity;
using RAGNET.Application.Configuration.Commands;
using RAGNET.Application.Subscriptions.Services;
using RAGNET.Domain.SharedKernel.Users;
using RAGNET.Domain.TokenWallets;
using RAGNET.Domain.Users;
using RAGNET.Domain.Users.Subscriptions;

namespace RAGNET.Application.Users.Commands.RegisterUser
{
    public class RegisterUserCommandHandler(
        UserManager<User> userManager,
        IPaymentGateway paymentGateway
    ) : ICommandHandler<RegisterUserCommand, (string userId, IEnumerable<string> Errors)>
    {
        private readonly UserManager<User> _userManager = userManager;
        private readonly IPaymentGateway _paymentGateway = paymentGateway;
        public async Task<(string userId, IEnumerable<string> Errors)> Handle(RegisterUserCommand request, CancellationToken cancellationToken)
        {
            var firstName = new Name(request.FirstName);
            var lastName = new Name(request.LastName);
            var email = new Email(request.Email);
            var userName = new UserName(request.Email);

            var user = User.Create(firstName, lastName, userName, email);

            user.AddWallet(TokenWallet.Create(
                user.Id
            ));

            user.AddSubscription(Subscription.Create(
                user.Id
            ));

            var customerId = await _paymentGateway.CreateCustomerAsync(user.Id, user.FirstName, user.LastName, email);

            user.AddCustomerId(customerId);

            var result = await _userManager.CreateAsync(user, request.Password);

            if (!result.Succeeded)
                return (string.Empty, result.Errors.Select(e => e.Description));

            await _userManager.UpdateAsync(user);

            return (user.Id, []);
        }
    }
}