using RAGNet.Domain.Services;
using RAGNET.Domain.Entities.Jobs;
using RAGNET.Domain.Repositories;

namespace RAGNET.Infrastructure.Workers.Handlers
{
    public class UpdateWorkflowHandler(IWorkflowRepository workflowRepository, IJobStatusRepository jobStatusRepository) : BaseJobProcessingHandler
    {
        public readonly IWorkflowRepository _workflowRepository = workflowRepository;
        public readonly IJobStatusRepository _jobStatusRepository = jobStatusRepository;
        public override async Task HandleAsync(EmbeddingJob job, CancellationToken ct)
        {

            var workflow = job.Context.Workflow;
            workflow.DocumentsCount++;
            await _workflowRepository.UpdateByApiKey(workflow, workflow.ApiKey);
            await _jobStatusRepository.MarkAsCompletedAsync(job.JobId);

            await base.HandleAsync(job, ct);
        }
    }
}