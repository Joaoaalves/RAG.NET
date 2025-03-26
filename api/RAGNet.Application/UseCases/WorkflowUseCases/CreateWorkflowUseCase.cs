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
    public class CreateWorkflowUseCase(IWorkflowRepository workflowRepository, IChunkerRepository chunkerRepository, IEmbeddingProviderConfigRepository embeddingProviderConfigRepository, IVectorDatabaseService vectorDatabaseService) : ICreateWorkflowUseCase
    {
        private readonly IWorkflowRepository _workflowRepository = workflowRepository;
        private readonly IChunkerRepository _chunkerRepository = chunkerRepository;
        private readonly IEmbeddingProviderConfigRepository _embeddingProviderConfigRepository = embeddingProviderConfigRepository;
        private readonly IVectorDatabaseService _vectorDatabaseService = vectorDatabaseService;
        public async Task<Guid> Execute(WorkflowCreationDTO dto, User user)
        {
            var workflow = dto.ToWorkflowFromCreationDTO(user);
            await _workflowRepository.AddAsync(workflow);

            var embeddingProvider = dto.EmbeddingProvider.ToEmbeddingProviderConfigFromEmbeddingProviderConfigDTO(workflow.Id);
            await _embeddingProviderConfigRepository.AddAsync(embeddingProvider);

            var chunker = dto.ToChunkerFromWorkflowCreationDTO(workflow.Id);
            await _chunkerRepository.AddAsync(chunker);

            await _vectorDatabaseService.CreateCollectionAsync(workflow.CollectionId, embeddingProvider.VectorSize);
            return workflow.Id;
        }
    }
}