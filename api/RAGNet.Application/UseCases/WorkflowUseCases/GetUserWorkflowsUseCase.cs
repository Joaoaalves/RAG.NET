using RAGNET.Application.DTOs.Workflow;
using RAGNET.Application.DTOs.Chunker;
using RAGNET.Domain.Repositories;
using RAGNET.Application.Mappers;

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
                var chunker = workflow.Chunkers.FirstOrDefault() ?? throw new Exception("Chunker não encontrado.");

                var settings = chunker.Metas.ToDictionary(m => m.Key, m => m.Value);

                workflowsDTO.Add(
                    // Converts to WorkflowDetailsDTO
                    workflow.ToWorkflowDetailsDTOFromWorkflow(
                        chunker.StrategyType,
                        settings.ToChunkerSettingsDTOfromDictionary()
                    ));
            }

            return workflowsDTO;
        }
    }
}
