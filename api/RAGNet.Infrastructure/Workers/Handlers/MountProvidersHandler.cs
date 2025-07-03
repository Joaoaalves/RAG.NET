using RAGNET.Application.Infrastructure.Providers.Conversation;
using RAGNET.Application.Infrastructure.Providers.Embedding;
using RAGNET.Application.ProviderApiKeys.Services;
using RAGNET.Application.Workflows.Commands.EnqueueEmbeddingJob.Jobs;
using RAGNET.Infrastructure.Jobs;

namespace RAGNET.Infrastructure.Workers.Handlers
{
    public class MountProvidersHandler(
        IApiKeyResolverService apiKeyResolver,
        IEmbedderFactory embedderFactory,
        IConversationProviderFactory chatCompletionFactory
    ) : BaseJobProcessingHandler
    {

        private readonly IApiKeyResolverService _apiKeyResolver = apiKeyResolver;

        private readonly IEmbedderFactory _embedderFactory = embedderFactory;
        private readonly IConversationProviderFactory _chatCompletionFactory = chatCompletionFactory;

        public override async Task HandleAsync(EmbeddingJob job, CancellationToken ct)
        {
            var workflow = job.Context.Workflow;

            var convoKey = await _apiKeyResolver.ResolveForUserAsync(
                job.UserId,
                workflow.ConversationProviderConfig!.Provider);

            var embedKey = await _apiKeyResolver.ResolveForUserAsync(
                job.UserId,
                workflow.EmbeddingProviderConfig.Provider
            );

            job.Context.EmbeddingProviderService = _embedderFactory.CreateEmbeddingService(embedKey, workflow.EmbeddingProviderConfig);

            job.Context.ConversationProviderService = _chatCompletionFactory.CreateCompletionService(convoKey, workflow.ConversationProviderConfig);

            await base.HandleAsync(job, ct);
        }
    }
}