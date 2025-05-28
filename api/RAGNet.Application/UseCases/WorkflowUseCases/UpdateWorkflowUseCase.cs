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
            var workflow = await _workflowRepository.GetWithRelationsAsync(workflowId, user.Id) ?? throw new Exception("Invalid workflow Id");

            if (dto.ConversationProvider != null)
            {
                var conversationProviderConfig = dto.ConversationProvider.ToConversationProviderConfig(Guid.NewGuid());

                _conversationProviderResolver.Resolve(conversationProviderConfig);

                workflow.ConversationProviderConfig = conversationProviderConfig;
            }

            if (dto.EmbeddingProvider != null)
            {
                var embeddingProviderConfig = dto.EmbeddingProvider.ToEmbeddingProviderConfig(Guid.NewGuid());

                _embeddingProviderResolver.Resolve(embeddingProviderConfig);

                workflow.EmbeddingProviderConfig = embeddingProviderConfig;
            }

            if (dto.Name != null)
            {
                workflow.Name = dto.Name;
            }

            if (dto.Description != null)
            {
                workflow.Description = dto.Description;
            }

            await _workflowRepository.UpdateAsync(workflow, user.Id);
            return workflow;
        }
    }
}