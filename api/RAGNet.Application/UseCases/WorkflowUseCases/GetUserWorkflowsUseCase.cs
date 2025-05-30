using RAGNET.Application.DTOs.Workflow;
using RAGNET.Application.DTOs.Chunker;
using RAGNET.Domain.Repositories;
using RAGNET.Application.Mappers;
using RAGNET.Application.DTOs.QueryEnhancer;

namespace RAGNET.Application.UseCases.WorkflowUseCases
{
    public interface IGetUserWorkflowsUseCase
    {
        Task<List<WorkflowDetailsDTO>> Execute(string userId);
    }

    public class GetUserWorkflowsUseCase(IWorkflowRepository workflowRepository) : IGetUserWorkflowsUseCase
    {
        private readonly IWorkflowRepository _workflowRepository = workflowRepository;

        public async Task<List<WorkflowDetailsDTO>> Execute(string userId)
        {
            var workflows = await _workflowRepository.GetUserWorkflows(userId);
            var workflowsDTO = new List<WorkflowDetailsDTO>();

            foreach (var workflow in workflows)
            {
                var chunker = workflow.Chunker ?? throw new Exception("Chunker not set.");

                var settings = chunker.Metas.ToDictionary(m => m.Key, m => m.Value);
                var embeddingProvider = workflow.EmbeddingProviderConfig ?? throw new Exception("Embedding Provider not set.");

                var conversationProvider = workflow.ConversationProviderConfig ?? throw new Exception("Conversation Provider not set.");

                workflowsDTO.Add(
                    workflow.ToWorkflowDetailsDTOFromWorkflow(
                        chunker.StrategyType,
                        settings.ToChunkerSettingsDTOfromDictionary(),
                        embeddingProvider.ToDTOFromEmbeddingProviderConfig(),
                        conversationProvider.ToDTOFromConversationProviderConfig(),
                        workflow.QueryEnhancers.ToDTOList(),
                        workflow.CallbackUrls.ToDTOList()
                    ));

            }

            return workflowsDTO;
        }
    }
}
