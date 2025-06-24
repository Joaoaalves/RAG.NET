using RAGNET.Domain.TokenWallets;

namespace RAGNET.Application.TokenWallets.Services.Consumption
{
    public interface ITokenConsumerContext
    {
        Task ConsumeAsync(
            ITokenConsumptionStrategy strategy,
            TokenWallet wallet,
            CancellationToken cancellationToken = default
        );
    }
}