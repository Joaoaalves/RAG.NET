#pragma warning disable
using FluentValidation;
using RAGNET.Application.Infrastructure.Providers.Conversation.Validators;
using RAGNET.Application.Infrastructure.Providers.Embedding.Validators;

namespace RAGNET.Application.Workflows.Commands.UpdateWorkflow
{
    public class UpdateWorkflowCommandValidator : AbstractValidator<UpdateWorkflowCommand>
    {
        public UpdateWorkflowCommandValidator()
        {
            RuleFor(w => w.Name).MinimumLength(5).When(w => w.Name is not null);
            RuleFor(w => w.Description).MinimumLength(5).When(w => w.Description is not null);
            RuleFor(w => w.IsActive).NotNull().When(w => w.IsActive.HasValue);

            RuleFor(w => w.EmbeddingProviderConfig)
                .SetValidator(new EmbeddingProviderConfigValidator())
                .When(w => w.EmbeddingProviderConfig is not null);

            RuleFor(w => w.ConversationProviderConfig)
                .SetValidator(new ConversationProviderConfigValidator())
                .When(w => w.ConversationProviderConfig is not null);
        }
    }
}