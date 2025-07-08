using RAGNET.Application.Workflows.Commands.EnqueueEmbeddingJob.Jobs;
using RAGNET.Domain.Users;
using RAGNET.Domain.Users.ApiKeys;
using RAGNET.Domain.Workflows;
using RAGNET.Infrastructure.Jobs;

namespace RAGNET.Infrastructure.Workers.Embedding.Handlers
{
    public class InitializeJobHandler(IJobStatusRepository jobStatusRepository, IWorkflowRepository workflowRepository, IUserRepository userRepository) : BaseJobProcessingHandler
    {
        private readonly IJobStatusRepository _jobStatusRepository = jobStatusRepository;
        private readonly IWorkflowRepository _workflowRepository = workflowRepository;
        private readonly IUserRepository _userRepository = userRepository;
        public override async Task HandleAsync(EmbeddingJob job, CancellationToken ct)
        {
            var workflow = await _workflowRepository.GetByApiKey(
                new ApiKey(job.ApiKey)
            ) ?? throw new Exception("Workflow not found");
            var user = await _userRepository.GetByIdAsync(job.UserId) ?? throw new Exception("User not found.");
            job.Context.User = user;
            job.Context.Workflow = workflow;

            await _jobStatusRepository.SetPendingAsync(job.JobId);

            await base.HandleAsync(job, ct);
        }
    }
}