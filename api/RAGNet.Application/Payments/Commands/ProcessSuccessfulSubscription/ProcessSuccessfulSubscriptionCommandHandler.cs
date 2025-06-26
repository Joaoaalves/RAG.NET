using RAGNET.Application.Configuration.Commands;
using RAGNET.Domain.SeedWork;
using RAGNET.Domain.SharedKernel.Tokens;
using RAGNET.Domain.TokenWallets;

namespace RAGNET.Application.Payments.Commands.ProcessSuccessfulSubscription
{
    public class ProcessSuccessfulSubscriptionCommandHandler(
        ITokenWalletRepository tokenWalletRepository,
        IUnitOfWork unitOfWork
    ) : ICommandHandler<ProcessSuccessfulSubscriptionCommand, Unit>
    {
        private readonly ITokenWalletRepository _tokenWalletRepository = tokenWalletRepository;
        private readonly IUnitOfWork _unitOfWork = unitOfWork;
        public async Task<Unit> Handle(ProcessSuccessfulSubscriptionCommand request, CancellationToken cancellationToken)
        {
            var wallet = await _tokenWalletRepository.GetByUserIdAsync(request.UserId) ?? throw new ApplicationException("Wallet not found");

            wallet.AddPaidTokens(TokenAmount.MonthlyPaidQuota);

            await _unitOfWork.CommitAsync(cancellationToken);

            return Unit.Value;
        }
    }
}