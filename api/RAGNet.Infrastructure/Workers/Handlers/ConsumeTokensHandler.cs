using RAGNET.Application.Chunkers.Services;
using RAGNET.Application.TokenWallets.Services.Consumption;
using RAGNET.Application.TokenWallets.Services.Consumption.Strategies;
using RAGNET.Application.Workflows.Commands.EnqueueEmbeddingJob.Contexts;
using RAGNET.Application.Workflows.Commands.EnqueueEmbeddingJob.Jobs;
using RAGNET.Domain.TokenWallets;
using RAGNET.Domain.Workflows;
using RAGNET.Infrastructure.Jobs;

namespace RAGNET.Infrastructure.Workers.Handlers
{
    public class ConsumeTokensHandler(
        ITokenConsumerContext tokenConsumerContext
    ) : BaseJobProcessingHandler
    {
        private readonly ITokenConsumerContext _tokenConsumerContext = tokenConsumerContext;
        public override async Task HandleAsync(EmbeddingJob job, CancellationToken ct)
        {
            var (workflow, wallet, chunker, totalPages) = ExtractContext(job.Context);

            var tokenConsumptionStrategy = new ChunkerConsumptionStrategy(
                workflow,
                chunker,
                totalPages
            );

            await _tokenConsumerContext.ConsumeAsync(
                workflow,
                wallet,
                tokenConsumptionStrategy,
                ct
            );

            await base.HandleAsync(job, ct);
        }

        private static (Workflow, TokenWallet, ITextChunkerService, int) ExtractContext(EmbeddingJobContext context)
        {
            return (context.Workflow, context.User.TokenWallet, context.TextChunkerService, context.Document.Pages.Count);
        }
    }
}