using RAGNET.Application.DTOs.Workflow;
using RAGNET.Application.Mappers;
using RAGNET.Domain.Entities;
using RAGNET.Domain.Repositories;

namespace RAGNET.Application.UseCases.WorkflowUseCases
{
    public interface ICreateWorkflowUseCase
    {
        Task<Guid> Execute(WorkflowCreationDTO dto, User user);
    }
    public class CreateWorkflowUseCase(IWorkflowRepository workflowRepository, IChunkerRepository chunkerRepository) : ICreateWorkflowUseCase
    {
        private readonly IWorkflowRepository _workflowRepository = workflowRepository;
        private readonly IChunkerRepository _chunkerRepository = chunkerRepository;


        public async Task<Guid> Execute(WorkflowCreationDTO dto, User user)
        {
            var workflow = dto.ToWorkflowFromCreationDTO(user);

            await _workflowRepository.AddAsync(workflow);

            var chunker = dto.ToChunkerFromWorkflowCreationDTO(workflow.Id);

            await _chunkerRepository.AddAsync(chunker);
            return workflow.Id;
        }
    }
}