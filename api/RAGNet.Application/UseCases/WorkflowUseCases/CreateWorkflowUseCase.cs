using RAGNET.Application.DTOs.Workflow;
using RAGNET.Application.Mappers;
using RAGNET.Domain.Entities;
using RAGNET.Domain.Repositories;
using RAGNET.Domain.Services;

namespace RAGNET.Application.UseCases.WorkflowUseCases
{
    public interface ICreateWorkflowUseCase
    {
        Task<Guid> Execute(WorkflowCreationDTO dto, User user);
    }
    public class CreateWorkflowUseCase(IWorkflowRepository workflowRepository,
        IChunkerRepository chunkerRepository, IEmbeddingProviderConfigRepository embeddingProviderConfigRepository,
         IVectorDatabaseService vectorDatabaseService, IEmbeddingProviderResolver embeddingProviderResolver,
          IConversationProviderConfigRepository conversationProviderConfigRepository, IConversationProviderResolver conversationProviderResolver) : ICreateWorkflowUseCase
    {
        private readonly IWorkflowRepository _workflowRepository = workflowRepository;
        private readonly IChunkerRepository _chunkerRepository = chunkerRepository;
        private readonly IEmbeddingProviderConfigRepository _embeddingProviderConfigRepository = embeddingProviderConfigRepository;
        private readonly IVectorDatabaseService _vectorDatabaseService = vectorDatabaseService;
        private readonly IEmbeddingProviderResolver _embeddingProviderResolver = embeddingProviderResolver;
        private readonly IConversationProviderConfigRepository _conversationProviderConfigRepository = conversationProviderConfigRepository;
        private readonly IConversationProviderResolver _conversationProviderResolver = conversationProviderResolver;
        public async Task<Guid> Execute(WorkflowCreationDTO dto, User user)
        {
            var embeddingModel = _embeddingProviderResolver.Resolve(dto.EmbeddingProvider.ToEmbeddingProviderConfigFromEmbeddingProviderConfigDTO(Guid.NewGuid()));
            dto.EmbeddingProvider.VectorSize = embeddingModel.VectorSize;

            _conversationProviderResolver.Resolve(dto.ConversationProvider.ToConversationProviderConfigFromConversationProviderConfigDTO(Guid.NewGuid()));

            var workflow = dto.ToWorkflowFromCreationDTO(user);
            await _workflowRepository.AddAsync(workflow);

            // Conversation Provider Config
            var conversationProvider = dto.ConversationProvider.ToConversationProviderConfigFromConversationProviderConfigDTO(workflow.Id);
            await _conversationProviderConfigRepository.AddAsync(conversationProvider);

            // Embedding Provider Config
            var embeddingProvider = dto.EmbeddingProvider.ToEmbeddingProviderConfigFromEmbeddingProviderConfigDTO(workflow.Id);
            await _embeddingProviderConfigRepository.AddAsync(embeddingProvider);

            // Chunker
            var chunker = dto.ToChunkerFromWorkflowCreationDTO(workflow.Id);
            await _chunkerRepository.AddAsync(chunker);

            // Vector Database
            await _vectorDatabaseService.CreateCollectionAsync(workflow.CollectionId, embeddingProvider.VectorSize);
            return workflow.Id;
        }
    }
}