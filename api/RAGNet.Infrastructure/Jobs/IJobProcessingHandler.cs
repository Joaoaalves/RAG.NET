using RAGNET.Application.Workflows.Commands.EnqueueEmbeddingJob.Jobs;

namespace RAGNET.Infrastructure.Jobs
{
    public interface IJobProcessingHandler
    {
        IJobProcessingHandler SetNext(IJobProcessingHandler next);
        Task HandleAsync(EmbeddingJob job, CancellationToken ct);
    }
}