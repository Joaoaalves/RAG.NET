using RAGNET.Domain.Users;
using RAGNET.Domain.SeedWork;

using RAGNET.Domain.Workflows;

using RAGNET.Application.Providers;
using RAGNET.Application.DTOs.Workflow;
using RAGNET.Application.Mappers;

namespace RAGNET.Application.UseCases.WorkflowUseCases
{
    public interface IUpdateWorkflowUseCase
    {
        Task<Workflow> Execute(WorkflowDetailsUpdateDTO dto, Guid workflowId, User user);
    }

    public class UpdateWorkflowUseCase(
        IWorkflowRepository workflowRepository,
        IConversationProviderResolver conversationProviderResolver,
        IEmbeddingProviderResolver embeddingProviderResolver,
        IUnitOfWork unitOfWork
    ) : IUpdateWorkflowUseCase
    {
        private readonly IWorkflowRepository _workflowRepository = workflowRepository;
        private readonly IEmbeddingProviderResolver _embeddingProviderResolver = embeddingProviderResolver;
        private readonly IConversationProviderResolver _conversationProviderResolver = conversationProviderResolver;
        private readonly IUnitOfWork _unitOfWork = unitOfWork;
        public async Task<Workflow> Execute(WorkflowDetailsUpdateDTO dto, Guid workflowId, User user)
        {
            var workflow = await _workflowRepository.GetByIdAsync(workflowId, user.Id)
                           ?? throw new Exception("Invalid workflow ID");

            if (dto.ConversationProvider is not null)
            {
                var config = dto.ConversationProvider.ToConversationProviderConfig(workflow.Id);
                _conversationProviderResolver.Resolve(config);
                workflow.UpdateConversationProviderConfig(config);
            }

            if (dto.EmbeddingProvider is not null)
            {
                var config = dto.EmbeddingProvider.ToEmbeddingProviderConfig(workflow.Id);
                _embeddingProviderResolver.Resolve(config);
                workflow.UpdateEmbeddingProviderConfig(config);

            }

            if (!string.IsNullOrWhiteSpace(dto.Name))
                workflow.Rename(dto.Name);

            if (!string.IsNullOrWhiteSpace(dto.Description))
                workflow.UpdateDescription(dto.Description);

            if (dto.IsActive is not null)
                workflow.SetActivationState(dto.IsActive.Value);

            await _workflowRepository.UpdateAsync(workflow, user.Id);
            await _unitOfWork.CommitAsync();

            return workflow;
        }
    }
}
