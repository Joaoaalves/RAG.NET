using RAGNET.Application.DTOs.Workflow;
using RAGNET.Application.Mappers;
using RAGNET.Domain.Workflows;

namespace RAGNET.Application.UseCases.WorkflowUseCases
{
    public interface IGetWorkflowUseCase
    {
        Task<WorkflowDetailsDTO> Execute(WorkflowId workflowId, string userId);
    }

    public class GetWorkflowUseCase(IWorkflowRepository workflowRepository) : IGetWorkflowUseCase
    {
        private readonly IWorkflowRepository _workflowRepository = workflowRepository;

        public async Task<WorkflowDetailsDTO> Execute(WorkflowId workflowId, string userId)
        {
            var workflow = await _workflowRepository.GetByIdAsync(workflowId, userId) ?? throw new Exception("Workflow não encontrado.");

            var chunker = workflow.Chunker ?? throw new Exception("Chunker não encontrado.");

            var settings = chunker.Metas.ToDictionary(m => m.Key, m => m.Value);
            var embeddingProvider = workflow.EmbeddingProviderConfig ?? throw new Exception("Embedding Provider não setado");

            var conversationProvider = workflow.ConversationProviderConfig ?? throw new Exception("Conversation Provider not set.");

            return workflow.ToWorkflowDetailsDTOFromWorkflow(
                chunker.StrategyType,
                settings.ToChunkerSettingsDTOfromDictionary(),
                embeddingProvider.ToDTOFromEmbeddingProviderConfig(),
                conversationProvider.ToDTOFromConversationProviderConfig(),
                workflow.QueryEnhancers.ToDTOList(),
                workflow.CallbackUrls.ToDTOList()
                );

        }
    }
}
