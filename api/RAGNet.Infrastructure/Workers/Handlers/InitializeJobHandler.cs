using RAGNET.Domain.TokenWallets;
using RAGNET.Domain.Users;
using RAGNET.Domain.Workflows;
using RAGNET.Infrastructure.Jobs;

namespace RAGNET.Infrastructure.Workers.Handlers
{
    public class InitializeJobHandler(IJobStatusRepository jobStatusRepository, IWorkflowRepository workflowRepository, IUserRepository userRepository, ITokenWalletRepository tokenWalletRepository) : BaseJobProcessingHandler
    {
        private readonly IJobStatusRepository _jobStatusRepository = jobStatusRepository;
        private readonly IWorkflowRepository _workflowRepository = workflowRepository;
        private readonly ITokenWalletRepository _tokenWalletRepository = tokenWalletRepository;
        private readonly IUserRepository _userRepository = userRepository;
        public override async Task HandleAsync(EmbeddingJob job, CancellationToken ct)
        {
            var workflow = await _workflowRepository.GetByApiKey(job.ApiKey) ?? throw new Exception("Workflow not found");
            var user = await _userRepository.GetByIdAsync(job.UserId) ?? throw new Exception("User not found.");
            var wallet = await _tokenWalletRepository.GetByUserIdAsync(workflow.UserId) ?? throw new Exception("Wallet not Found");

            job.Context.User = user;
            job.Context.Workflow = workflow;
            job.Context.Wallet = wallet;

            await _jobStatusRepository.SetPendingAsync(job.JobId);

            await base.HandleAsync(job, ct);
        }
    }
}