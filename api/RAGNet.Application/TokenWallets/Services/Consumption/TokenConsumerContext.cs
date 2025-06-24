using RAGNET.Domain.SeedWork;
using RAGNET.Domain.TokenWallets;

namespace RAGNET.Application.TokenWallets.Services.Consumption
{
    public class TokenConsumerContext(
    ITokenWalletRepository walletRepository,
    IUnitOfWork unitOfWork
) : ITokenConsumerContext
    {
        private readonly ITokenWalletRepository _walletRepository = walletRepository;
        private readonly IUnitOfWork _unitOfWork = unitOfWork;

        public async Task ConsumeAsync(
            ITokenConsumptionStrategy strategy,
            TokenWallet wallet,
            CancellationToken cancellationToken = default
        )
        {
            var cost = strategy.CalculateCost();

            wallet.Consume(cost, strategy.Operation, strategy.GetContextInfo());

            Console.WriteLine($"User: {wallet.UserId} consumed {cost} tokens");
            await _walletRepository.UpdateAsync(wallet);

            await _unitOfWork.CommitAsync(cancellationToken);
        }
    }

}