using RAGNET.Application.DTOs.Workflow;
using RAGNET.Application.DTOs.Chunker;
using RAGNET.Domain.Repositories;
using RAGNET.Application.Mappers;

namespace RAGNET.Application.UseCases.WorkflowUseCases
{
    public interface IGetWorkflowUseCase
    {
        Task<WorkflowDetailsDTO> Execute(Guid workflowId);
    }

    public class GetWorkflowUseCase(IWorkflowRepository workflowRepository) : IGetWorkflowUseCase
    {
        private readonly IWorkflowRepository _workflowRepository = workflowRepository;

        public async Task<WorkflowDetailsDTO> Execute(Guid workflowId)
        {
            var workflow = await _workflowRepository.GetWithRelationsAsync(workflowId) ?? throw new Exception("Workflow não encontrado.");

            var chunker = workflow.Chunkers.FirstOrDefault() ?? throw new Exception("Chunker não encontrado.");

            var settings = chunker.Metas.ToDictionary(m => m.Key, m => m.Value);

            return workflow.ToWorkflowDetailsDTOFromWorkflow(chunker.StrategyType, settings.ToChunkerSettingsDTOfromDictionary());
        }
    }
}
