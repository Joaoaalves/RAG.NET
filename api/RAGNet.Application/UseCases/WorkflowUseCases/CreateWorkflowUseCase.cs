using RAGNET.Domain.Chunkers;
using RAGNET.Domain.Users;
using RAGNET.Domain.SeedWork;

using RAGNET.Domain.Workflows;

using RAGNET.Application.DTOs.Workflow;
using RAGNET.Application.Mappers;
using RAGNET.Application.Providers;

namespace RAGNET.Application.UseCases.WorkflowUseCases
{
    public interface ICreateWorkflowUseCase
    {
        Task<Guid> Execute(WorkflowCreationDTO dto, User user);
    }
    public class CreateWorkflowUseCase(IWorkflowRepository workflowRepository,
        IChunkerRepository chunkerRepository,
        IVectorDatabaseService vectorDatabaseService,
        IEmbeddingProviderResolver embeddingProviderResolver,
        IConversationProviderResolver conversationProviderResolver,
        IUnitOfWork unitOfWork) : ICreateWorkflowUseCase
    {
        private readonly IWorkflowRepository _workflowRepository = workflowRepository;
        private readonly IChunkerRepository _chunkerRepository = chunkerRepository;
        private readonly IVectorDatabaseService _vectorDatabaseService = vectorDatabaseService;
        private readonly IEmbeddingProviderResolver _embeddingProviderResolver = embeddingProviderResolver;
        private readonly IConversationProviderResolver _conversationProviderResolver = conversationProviderResolver;
        private readonly IUnitOfWork _unitOfWork = unitOfWork;
        public async Task<Guid> Execute(WorkflowCreationDTO dto, User user)
        {
            var workflowId = Guid.NewGuid();

            var embeddingModel = _embeddingProviderResolver.Resolve(
                dto.EmbeddingProvider.ToEmbeddingProviderConfig(workflowId)
            );

            var vectorSize = embeddingModel.VectorSize;

            _conversationProviderResolver.Resolve(
                dto.ConversationProvider.ToConversationProviderConfig(workflowId)
            );

            // Conversation Provider Config
            var conversationProvider = dto.ConversationProvider.ToConversationProviderConfig(workflowId);

            // Embedding Provider Config
            var embeddingProvider = dto.EmbeddingProvider.ToEmbeddingProviderConfig(workflowId, vectorSize);

            // Chunker
            var chunker = dto.ToChunkerFromWorkflowCreationDTO(workflowId, user.Id);
            await _chunkerRepository.AddAsync(chunker);

            var workflow = new WorkflowBuilder()
                .WithName(dto.Name)
                .WithDescription(dto.Description)
                .ForUser(user.Id)
                .WithApiKey(Guid.NewGuid().ToString("N"))
                .WithCollectionId(Guid.NewGuid())
                .WithChunker(chunker)
                .WithConversationProvider(conversationProvider)
                .WithEmbeddingProvider(embeddingProvider)
                .Build(workflowId);

            await _workflowRepository.AddAsync(workflow);

            // Vector Database
            await _vectorDatabaseService.CreateCollectionAsync(
                workflow.CollectionId,
                embeddingProvider.VectorSize
            );

            await _unitOfWork.CommitAsync();

            return workflowId;
        }
    }
}