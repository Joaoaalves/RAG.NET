using RAGNET.Application.Infrastructure.Providers.Embedding;
using RAGNET.Application.Workflows.Commands.EnqueueEmbeddingJob.Jobs;
using RAGNET.Domain.SharedKernel.Plans.Policies;
using RAGNET.Infrastructure.Jobs;

namespace RAGNET.Infrastructure.Workers.Handlers
{
    public class MountChunkerHandler(
        IEmbeddingProcessingService embeddingService,
        SubscriptionPolicy subscriptionPolicy
    ) : BaseJobProcessingHandler
    {
        private readonly IEmbeddingProcessingService _embeddingService = embeddingService;
        private readonly SubscriptionPolicy _policy = subscriptionPolicy;
        public override async Task HandleAsync(EmbeddingJob job, CancellationToken ct)
        {
            var chunker = job.Context.Workflow.Chunker;
            var user = job.Context.User;

            if (!_policy.Allows(user, chunker.StrategyType))
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