using RAGNET.Domain.TokenWallets;
using RAGNET.Domain.Workflows;

namespace RAGNET.Application.TokenWallets.Services.Consumption
{
    public interface ITokenConsumerContext
    {
        Task ConsumeAsync(
            Workflow workflow,
            TokenWallet wallet,
            ITokenConsumptionStrategy strategy,
            CancellationToken cancellationToken = default
        );
    }
}