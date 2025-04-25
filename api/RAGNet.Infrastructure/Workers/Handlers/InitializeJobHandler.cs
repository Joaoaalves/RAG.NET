using RAGNet.Domain.Services;
using RAGNET.Domain.Entities.Jobs;
using RAGNET.Domain.Repositories;

namespace RAGNET.Infrastructure.Workers.Handlers
{
    public class InitializeJobHandler(IJobStatusRepository jobStatusRepository, IWorkflowRepository workflowRepository) : BaseJobProcessingHandler
    {
        public readonly IJobStatusRepository _jobStatusRepository = jobStatusRepository;
        public readonly IWorkflowRepository _workflowRepository = workflowRepository;
        public override async Task HandleAsync(EmbeddingJob job, CancellationToken ct)
        {
            var workflow = await _workflowRepository.GetWithRelationsByApiKey(job.ApiKey) ?? throw new Exception("Workflow not found");

            job.Context.Workflow = workflow;

            await _jobStatusRepository.SetPendingAsync(job.JobId);

            await base.HandleAsync(job, ct);
        }
    }
}