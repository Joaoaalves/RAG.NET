using RAGNET.Application.Configuration.Commands;
using RAGNET.Application.Infrastructure.Providers.Conversation;
using RAGNET.Application.Infrastructure.Providers.Conversation.Mappers;
using RAGNET.Application.Infrastructure.Providers.Embedding;
using RAGNET.Application.Infrastructure.Providers.Embedding.Mappers;
using RAGNET.Application.Workflows.Mappers;
using RAGNET.Application.Workflows.Queries.GetWorkflowDetails;

using RAGNET.Domain.SeedWork;
using RAGNET.Domain.Workflows;

namespace RAGNET.Application.Workflows.Commands.UpdateWorkflow
{
    public class UpdateWorkflowCommandHandler(
        IWorkflowRepository workflowRepository,
        IConversationProviderResolver conversationProviderResolver,
        IEmbeddingProviderResolver embeddingProviderResolver,
        IUnitOfWork unitOfWork
    ) : ICommandHandler<UpdateWorkflowCommand, WorkflowDetailsDTO>
    {

        private readonly IWorkflowRepository _workflowRepository = workflowRepository;
        private readonly IEmbeddingProviderResolver _embeddingProviderResolver = embeddingProviderResolver;
        private readonly IConversationProviderResolver _conversationProviderResolver = conversationProviderResolver;
        private readonly IUnitOfWork _unitOfWork = unitOfWork;
        public async Task<WorkflowDetailsDTO> Handle(UpdateWorkflowCommand request, CancellationToken cancellationToken)
        {
            var workflow = await _workflowRepository.GetByIdAsync(request.WorkflowId, request.User.Id)
                                      ?? throw new Exception("Invalid workflow ID");

            if (request.ConversationProviderConfig is not null)
            {
                var config = request.ConversationProviderConfig.ToConversationProviderConfig();
                _conversationProviderResolver.Resolve(config);
                workflow.UpdateConversationProviderConfig(config);
            }

            if (request.EmbeddingProviderConfig is not null)
            {
                var config = request.EmbeddingProviderConfig.ToEmbeddingProviderConfig();
                _embeddingProviderResolver.Resolve(config);
                workflow.UpdateEmbeddingProviderConfig(config);

            }

            if (!string.IsNullOrWhiteSpace(request.Name))
                workflow.Rename(request.Name);

            if (!string.IsNullOrWhiteSpace(request.Description))
                workflow.UpdateDescription(request.Description);

            if (request.IsActive is not null)
                workflow.SetActivationState(request.IsActive.Value);

            await _workflowRepository.UpdateAsync(workflow, request.User.Id);
            await _unitOfWork.CommitAsync(cancellationToken);

            return workflow.ToWorkflowDetailsDTO();
        }
    }
}