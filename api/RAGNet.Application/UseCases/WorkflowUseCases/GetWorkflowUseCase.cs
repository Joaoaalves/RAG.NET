using RAGNET.Application.DTOs.Workflow;
using RAGNET.Domain.Repositories;
using RAGNET.Application.Mappers;
using RAGNET.Application.DTOs.QueryEnhancer;

namespace RAGNET.Application.UseCases.WorkflowUseCases
{
    public interface IGetWorkflowUseCase
    {
        Task<WorkflowDetailsDTO> Execute(Guid workflowId, string userId);
    }

    public class GetWorkflowUseCase(IWorkflowRepository workflowRepository) : IGetWorkflowUseCase
    {
        private readonly IWorkflowRepository _workflowRepository = workflowRepository;

        public async Task<WorkflowDetailsDTO> Execute(Guid workflowId, string userId)
        {
            var workflow = await _workflowRepository.GetWithRelationsAsync(workflowId, userId) ?? throw new Exception("Workflow não encontrado.");

            var chunker = workflow.Chunker ?? throw new Exception("Chunker não encontrado.");

            var settings = chunker.Metas.ToDictionary(m => m.Key, m => m.Value);
            var embeddingProvider = workflow.EmbeddingProviderConfig ?? throw new Exception("Embedding Provider não setado");

            var conversationProvider = workflow.ConversationProviderConfig ?? throw new Exception("Conversation Provider not set.");
            List<QueryEnhancerDTO> queryEnhancers = [];

            foreach (var qe in workflow.QueryEnhancers)
            {
                var dto = qe.ToQueryEnhancerDTO();
                queryEnhancers.Add(dto);
            }

            return workflow.ToWorkflowDetailsDTOFromWorkflow(
                chunker.StrategyType,
                settings.ToChunkerSettingsDTOfromDictionary(),
                embeddingProvider.ToDTOFromEmbeddingProviderConfig(),
                conversationProvider.ToDTOFromConversationProviderConfig(),
                queryEnhancers
                );

        }
    }
}
