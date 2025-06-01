using RAGNET.Application.DTOs.Workflow;
using RAGNET.Application.Mappers;
using RAGNET.Domain.Entities;
using RAGNET.Domain.Repositories;
using RAGNET.Domain.Services;

namespace RAGNET.Application.UseCases.WorkflowUseCases
{
    public interface IUpdateWorkflowUseCase
    {
        Task<Workflow> Execute(WorkflowDetailsUpdateDTO dto, Guid workflowId, User user);
    }

    public class UpdateWorkflowUseCase(
        IWorkflowRepository workflowRepository,
        IConversationProviderResolver conversationProviderResolver,
        IEmbeddingProviderResolver embeddingProviderResolver
    ) : IUpdateWorkflowUseCase
    {
        private readonly IWorkflowRepository _workflowRepository = workflowRepository;
        private readonly IEmbeddingProviderResolver _embeddingProviderResolver = embeddingProviderResolver;
        private readonly IConversationProviderResolver _conversationProviderResolver = conversationProviderResolver;

        public async Task<Workflow> Execute(WorkflowDetailsUpdateDTO dto, Guid workflowId, User user)
        {
            var workflow = await _workflowRepository.GetWithRelationsAsync(workflowId, user.Id)
                           ?? throw new Exception("Invalid workflow ID");

            if (dto.ConversationProvider is not null)
            {
                var config = dto.ConversationProvider.ToConversationProviderConfig(Guid.NewGuid());
                _conversationProviderResolver.Resolve(config);
                workflow.ConversationProviderConfig = config;
            }

            if (dto.EmbeddingProvider is not null)
            {
                var config = dto.EmbeddingProvider.ToEmbeddingProviderConfig(Guid.NewGuid());
                _embeddingProviderResolver.Resolve(config);
                workflow.EmbeddingProviderConfig = config;
            }

            if (!string.IsNullOrWhiteSpace(dto.Name))
            {
                workflow.Name = dto.Name;
            }

            if (!string.IsNullOrWhiteSpace(dto.Description))
            {
                workflow.Description = dto.Description;
            }

            if (dto.IsActive.HasValue)
            {
                workflow.IsActive = dto.IsActive.Value;
            }

            await _workflowRepository.UpdateAsync(workflow, user.Id);
            return workflow;
        }
    }
}
