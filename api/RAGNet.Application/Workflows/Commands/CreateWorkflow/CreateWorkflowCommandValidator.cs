using FluentValidation;
using RAGNET.Application.Infrastructure.Providers.Conversation.Validators;
using RAGNET.Application.Infrastructure.Providers.Embedding.Validators;

namespace RAGNET.Application.Workflows.Commands.CreateWorkflow
{

    public class CreateWorkflowCommandValidator : AbstractValidator<CreateWorkflowCommand>
    {
        public CreateWorkflowCommandValidator()
        {
            RuleFor(c => c.Dto.Name).NotEmpty().MaximumLength(100);
            RuleFor(c => c.Dto.Description).NotEmpty().MaximumLength(500);

            RuleFor(c => c.Dto.Strategy).IsInEnum();

            RuleFor(c => c.Dto.ConversationProvider).SetValidator(
                new ConversationProviderConfigValidator()
            );

            RuleFor(c => c.Dto.EmbeddingProvider).SetValidator(
                new EmbeddingProviderConfigValidator()
            );

            RuleFor(c => c.Dto.Settings.Threshold).InclusiveBetween(0, 1);
            RuleFor(c => c.Dto.Settings.MaxChunkSize).InclusiveBetween(100, 1200);
        }

    }

}