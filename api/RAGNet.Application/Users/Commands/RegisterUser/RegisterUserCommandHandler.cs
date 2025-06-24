using Microsoft.AspNetCore.Identity;
using RAGNET.Application.Configuration.Commands;
using RAGNET.Domain.SeedWork;
using RAGNET.Domain.SharedKernel.Tokens;
using RAGNET.Domain.SharedKernel.Users;
using RAGNET.Domain.TokenWallets;
using RAGNET.Domain.Users;

namespace RAGNET.Application.Users.Commands.RegisterUser
{
    public class RegisterUserCommandHandler(
        UserManager<User> userManager,
        ITokenWalletRepository tokenWalletRepository,
        IUnitOfWork unitOfWork
    ) : ICommandHandler<RegisterUserCommand, (string userId, IEnumerable<string> Errors)>
    {
        private readonly UserManager<User> _userManager = userManager;
        private readonly ITokenWalletRepository _tokenWalletRepository = tokenWalletRepository;
        private readonly IUnitOfWork _unitOfWork = unitOfWork;
        public async Task<(string userId, IEnumerable<string> Errors)> Handle(RegisterUserCommand request, CancellationToken cancellationToken)
        {
            var firstName = new Name(request.FirstName);
            var lastName = new Name(request.LastName);
            var email = new Email(request.Email);
            var userName = new UserName(request.Email);

            var user = User.Create(firstName, lastName, userName, email);

            try
            {
                var walletData = TokenWallet.Create(
                    user.Id,
                    TokenAmount.FromDecimal(50),
                    TokenAmount.FromDecimal(0)
                );

                var wallet = await _tokenWalletRepository.AddAsync(walletData);

                user.AddWallet(wallet);

                var result = await _userManager.CreateAsync(user, request.Password);

                if (!result.Succeeded)
                    return (string.Empty, result.Errors.Select(e => e.Description));

                await _unitOfWork.CommitAsync(cancellationToken);

                return (user.Id, []);
            }
            catch
            {
                await _unitOfWork.RevertAsync();
                throw;
            }

        }
    }
}