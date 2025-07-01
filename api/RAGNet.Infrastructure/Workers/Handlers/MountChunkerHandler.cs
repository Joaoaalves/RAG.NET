using RAGNET.Application.Infrastructure.Providers.Embedding;
using RAGNET.Domain.SharedKernel.Plans.Policies;
using RAGNET.Infrastructure.Jobs;

namespace RAGNET.Infrastructure.Workers.Handlers
{
    public class MountChunkerHandler(
        IEmbeddingProcessingService embeddingService
    ) : BaseJobProcessingHandler
    {
        private readonly IEmbeddingProcessingService _embeddingService = embeddingService;
        public override async Task HandleAsync(EmbeddingJob job, CancellationToken ct)
        {
            var chunker = job.Context.Workflow.Chunker;
            var user = job.Context.User;

            if (!SubscriptionPolicy.AllowsChunker(user, chunker.StrategyType))
            {
                throw new InvalidOperationException("Your current subscription does not allow this operation.");
            }

            job.Context.TextChunkerService = _embeddingService.GetChunker(
                chunker,
                job.Context.ConversationProviderService
            );

            await base.HandleAsync(job, ct);
        }
    }
}