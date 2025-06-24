using RAGNET.Domain.SeedWork;
using RAGNET.Domain.TokenWallets;
using RAGNET.Domain.Workflows;

namespace RAGNET.Application.Configuration.Commands
{
    public class WorkflowAndWalletAwareCommand<TResult> : IWorkflowAware, IWalletAware, ICommand<TResult>
    {
        public Guid Id { get; } = Guid.NewGuid();
        public Workflow Workflow { get; private set; } = default!;
        public TokenWallet TokenWallet { get; private set; } = default!;
        public void InjectWallet(TokenWallet wallet)
        {
            TokenWallet = wallet;
        }

        public void InjectWorkflow(Workflow workflow)
        {
            Workflow = workflow;
        }
    }
}