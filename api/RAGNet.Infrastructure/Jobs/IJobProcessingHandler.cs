using RAGNET.Application.Workflows.Commands.EnqueueEmbeddingJob.Jobs;
using RAGNET.Infrastructure.Jobs.Queue;

namespace RAGNET.Infrastructure.Jobs
{
    public interface IJobProcessingHandler
    {
        void SetNext(IJobProcessingHandler next);
        Task HandleAsync(EmbeddingJob job, CancellationToken ct);
    }
}