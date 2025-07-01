using FluentValidation;
using RAGNET.Application.Infrastructure.Providers.Conversation.Validators;
using RAGNET.Application.Infrastructure.Providers.Embedding.Validators;
using RAGNET.Domain.SharedKernel.Plans.Policies;

namespace RAGNET.Application.Workflows.Commands.CreateWorkflow
{

    public class CreateWorkflowCommandValidator : AbstractValidator<CreateWorkflowCommand>
    {
        public CreateWorkflowCommandValidator()
        {
            RuleFor(x => x)
                .Must(command => SubscriptionPolicy.AllowsChunker(command.User, command.Strategy))
                .WithMessage("Your subscription plan does not allow this chunker strategy.");

            RuleFor(c => c.Name).NotEmpty().MaximumLength(100);
            RuleFor(c => c.Description).NotEmpty().MaximumLength(500);

            RuleFor(c => c.Strategy).IsInEnum();

            RuleFor(c => c.ConversationProvider).SetValidator(
                new ConversationProviderConfigValidator()
            );

            RuleFor(c => c.EmbeddingProvider).SetValidator(
                new EmbeddingProviderConfigValidator()
            );

            RuleFor(c => c.Settings.Threshold).InclusiveBetween(0, 1);
            RuleFor(c => c.Settings.MaxChunkSize).InclusiveBetween(100, 1200);
        }
    }
}