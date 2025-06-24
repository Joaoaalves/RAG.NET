using RAGNET.Domain.SeedWork;
using RAGNET.Domain.TokenWallets;
using RAGNET.Domain.Workflows;

namespace RAGNET.Application.TokenWallets.Services.Consumption
{
    public class TokenConsumerContext(
    ITokenWalletRepository walletRepository,
    IWorkflowRepository workflowRepository,
    IUnitOfWork unitOfWork
) : ITokenConsumerContext
    {
        private readonly ITokenWalletRepository _walletRepository = walletRepository;
        private readonly IWorkflowRepository _workflowRepository = workflowRepository;
        private readonly IUnitOfWork _unitOfWork = unitOfWork;

        public async Task ConsumeAsync(
            Workflow workflow,
            TokenWallet wallet,
            ITokenConsumptionStrategy strategy,
            CancellationToken cancellationToken = default
        )
        {
            var cost = strategy.CalculateCost();

            wallet.Consume(cost, strategy.Operation, strategy.GetContextInfo());
            workflow.IncreaseTokenUsage(cost);

            await _walletRepository.UpdateAsync(wallet);
            await _workflowRepository.UpdateAsync(workflow, wallet.UserId);

            await _unitOfWork.CommitAsync(cancellationToken);
        }
    }

}