using RAGNET.Domain.SharedKernel.Plans.Policies;

using RAGNET.Application.Infrastructure.Providers.Embedding;
using RAGNET.Application.Workflows.Commands.EnqueueEmbeddingJob.Jobs;

using RAGNET.Infrastructure.Jobs;
using RAGNET.Infrastructure.Jobs.Queue;
using RAGNET.Application.Chunkers.Factories;

namespace RAGNET.Infrastructure.Workers.Embedding.Handlers
{
    public class MountChunkerHandler(
        IEmbeddingProcessingService embeddingService,
        SubscriptionPolicy subscriptionPolicy,
        ITextChunkerFactory textChunkerFactory,
        IJobNotificationService realTimeNotifier
    ) : NotifierJobProcessingHandler(realTimeNotifier)
    {
        private readonly IEmbeddingProcessingService _embeddingService = embeddingService;
        private readonly SubscriptionPolicy _policy = subscriptionPolicy;
        private readonly ITextChunkerFactory _textChunkerFactory = textChunkerFactory;
        private readonly ProcessDTO _currentProcess = new()
        {
            Title = "Mounting Chunker",
            Progress = 0
        };


        public override async Task HandleAsync(EmbeddingJob job, CancellationToken ct)
        {
            await NotifyProgress(job, _currentProcess, ct);
            var (user, chunker) = job.Context;

            if (!_policy.Allows(user, chunker.StrategyType))
            {
                throw new InvalidOperationException(
                    "Your current subscription does not allow this operation."
                );
            }

            job.Context.TextChunkerService = _textChunkerFactory.CreateChunker(
                chunker,
                job.Context.ConversationProviderService
            );

            await base.HandleAsync(job, ct);
        }
    }
}