using RAGNET.Application.Workflows.Commands.EnqueueEmbeddingJob.Jobs;
using RAGNET.Domain.Workflows;
using RAGNET.Infrastructure.Jobs;

namespace RAGNET.Infrastructure.Workers.Embedding.Handlers
{
    public class UpdateWorkflowHandler(IWorkflowRepository workflowRepository, IJobStatusRepository jobStatusRepository) : BaseJobProcessingHandler
    {
        public readonly IWorkflowRepository _workflowRepository = workflowRepository;
        public readonly IJobStatusRepository _jobStatusRepository = jobStatusRepository;
        public override async Task HandleAsync(EmbeddingJob job, CancellationToken ct)
        {
            var workflow = job.Context.Workflow;
            await _workflowRepository.UpdateByApiKey(workflow, workflow.ApiKey);
            await _jobStatusRepository.MarkAsCompletedAsync(job.JobId);
            await base.HandleAsync(job, ct);
        }
    }
}