using RAGNET.Application.Configuration.Commands;
using RAGNET.Application.Infrastructure.Providers;
using RAGNET.Application.Infrastructure.Providers.Conversation;
using RAGNET.Application.Infrastructure.Providers.Conversation.Mappers;
using RAGNET.Application.Infrastructure.Providers.Embedding;
using RAGNET.Application.Infrastructure.Providers.Embedding.Mappers;

using RAGNET.Domain.Chunkers;
using RAGNET.Domain.SeedWork;
using RAGNET.Domain.SharedKernel.Metas;
using RAGNET.Domain.Workflows;

namespace RAGNET.Application.Workflows.Commands.CreateWorkflow
{
    public class CreateWorkflowCommandHandler(
        IWorkflowRepository workflowRepository,
        IChunkerRepository chunkerRepository,
        IVectorDatabaseService vectorDatabaseService,
        IEmbeddingProviderResolver embeddingProviderResolver,
        IConversationProviderResolver conversationProviderResolver,
        IUnitOfWork unitOfWork) : ICommandHandler<CreateWorkflowCommand, WorkflowId>
    {
        private readonly IWorkflowRepository _workflowRepository = workflowRepository;
        private readonly IChunkerRepository _chunkerRepository = chunkerRepository;
        private readonly IVectorDatabaseService _vectorDatabaseService = vectorDatabaseService;
        private readonly IEmbeddingProviderResolver _embeddingProviderResolver = embeddingProviderResolver;
        private readonly IConversationProviderResolver _conversationProviderResolver = conversationProviderResolver;
        private readonly IUnitOfWork _unitOfWork = unitOfWork;

        public async Task<WorkflowId> Handle(CreateWorkflowCommand request, CancellationToken cancellationToken)
        {
            var user = request.User;
            var workflowId = new WorkflowId();

            var embeddingModel = _embeddingProviderResolver.Resolve(
                request.EmbeddingProvider.ToEmbeddingProviderConfig()
            );

            var vectorSize = embeddingModel.VectorSize;

            _conversationProviderResolver.Resolve(request.ConversationProvider.ToConversationProviderConfig());

            var conversationProvider = request.ConversationProvider.ToConversationProviderConfig();
            var embeddingProvider = request.EmbeddingProvider.ToEmbeddingProviderConfig(vectorSize);

            var chunker = new ChunkerBuilder()
                .ForWorkflow(workflowId)
                .ForUser(user.Id)
                .WithStrategy(request.Strategy)
                .AddMeta(new Meta("threshold", request.Settings.Threshold.ToString()))
                .AddMeta(new Meta("evaluationPrompt", request.Settings.EvaluationPrompt))
                .AddMeta(new Meta("maxChunkSize", request.Settings.MaxChunkSize.ToString()))
                .Build();

            await _chunkerRepository.AddAsync(chunker);

            var workflow = new WorkflowBuilder()
                .WithName(request.Name)
                .WithDescription(request.Description)
                .ForUser(user.Id)
                .WithApiKey(Guid.NewGuid().ToString("N"))
                .WithCollectionId(Guid.NewGuid())
                .WithChunker(chunker)
                .WithConversationProvider(conversationProvider)
                .WithEmbeddingProvider(embeddingProvider)
                .Build(workflowId);

            await _workflowRepository.AddAsync(workflow);

            await _vectorDatabaseService.CreateCollectionAsync(
                workflow.CollectionId,
                embeddingProvider.VectorSize
            );

            await _unitOfWork.CommitAsync(cancellationToken);

            return workflowId;
        }
    }
}