using RAGNET.Application.Configuration.Commands;
using RAGNET.Application.Infrastructure.Providers.Conversation;
using RAGNET.Application.Infrastructure.Providers.Conversation.Mappers;
using RAGNET.Application.Infrastructure.Providers.Embedding;
using RAGNET.Application.Infrastructure.Providers.Embedding.Mappers;
using RAGNET.Application.Infrastructure.Providers.VectorDatabases;
using RAGNET.Application.ProviderApiKeys.Services;
using RAGNET.Domain.Chunkers;
using RAGNET.Domain.SeedWork;
using RAGNET.Domain.SharedKernel.Metas;
using RAGNET.Domain.VectorStorages;
using RAGNET.Domain.Workflows;
using RAGNET.Domain.Workflows.VectorStorageConfigs;

namespace RAGNET.Application.Workflows.Commands.CreateWorkflow
{
    public class CreateWorkflowCommandHandler(
        IWorkflowRepository workflowRepository,
        IChunkerRepository chunkerRepository,
        IApiKeyResolverService apiKeyResolverService,
        IVectorDatabaseFactory vectorDatabaseFactory,
        IEmbeddingProviderResolver embeddingProviderResolver,
        IConversationProviderResolver conversationProviderResolver,
        IUnitOfWork unitOfWork) : ICommandHandler<CreateWorkflowCommand, WorkflowId>
    {
        private readonly IWorkflowRepository _workflowRepository = workflowRepository;
        private readonly IChunkerRepository _chunkerRepository = chunkerRepository;
        private readonly IApiKeyResolverService _apiKeyResolverService = apiKeyResolverService;
        private readonly IVectorDatabaseFactory _vectorDatabaseFactory = vectorDatabaseFactory;
        private readonly IEmbeddingProviderResolver _embeddingProviderResolver = embeddingProviderResolver;
        private readonly IConversationProviderResolver _conversationProviderResolver = conversationProviderResolver;
        private readonly IUnitOfWork _unitOfWork = unitOfWork;

        public async Task<WorkflowId> Handle(CreateWorkflowCommand request, CancellationToken cancellationToken)
        {
            var user = request.User;
            var workflowId = new WorkflowId();
            var vectorStorageId = new VectorStorageId(request.VectorStorageId);

            var embeddingModel = _embeddingProviderResolver.Resolve(
                request.EmbeddingProvider.ToEmbeddingProviderConfig()
            );

            var vectorSize = embeddingModel.VectorSize;

            _conversationProviderResolver.Resolve(request.ConversationProvider.ToConversationProviderConfig());

            var conversationProvider = request.ConversationProvider.ToConversationProviderConfig();
            var embeddingProvider = request.EmbeddingProvider.ToEmbeddingProviderConfig(vectorSize);

            var vectorStorageConfig = VectorStorageConfig.Create(
                vectorStorageId,
                Guid.NewGuid(),
                (uint)embeddingModel.VectorSize,
                workflowId
            );
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
                .WithCollectionId(vectorStorageConfig.CollectionId)
                .WithChunker(chunker)
                .WithConversationProvider(conversationProvider)
                .WithEmbeddingProvider(embeddingProvider)
                .WithVectorStorage(vectorStorageConfig)
                .Build(workflowId);

            await _workflowRepository.AddAsync(workflow);

            var vectorDatabaseService = await _vectorDatabaseFactory.CreateVectorDatabaseServiceAsync(
                vectorStorageId,
                user.Id
            );

            await vectorDatabaseService.CreateCollectionAsync(
                workflow.CollectionId,
                embeddingProvider.VectorSize
            );

            await _unitOfWork.CommitAsync(cancellationToken);

            return workflowId;
        }
    }
}