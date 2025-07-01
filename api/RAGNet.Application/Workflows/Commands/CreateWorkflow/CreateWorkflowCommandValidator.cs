using FluentValidation;
using RAGNET.Application.Infrastructure.Providers.Conversation.Validators;
using RAGNET.Application.Infrastructure.Providers.Embedding.Validators;
using RAGNET.Domain.SharedKernel.Plans.Policies;

namespace RAGNET.Application.Workflows.Commands.CreateWorkflow
{

    public class CreateWorkflowCommandValidator : AbstractValidator<CreateWorkflowCommand>
    {
        private readonly SubscriptionPolicy _subscriptionPolicy;

        public CreateWorkflowCommandValidator(SubscriptionPolicy subscriptionPolicy)
        {
            _subscriptionPolicy = subscriptionPolicy;

            RuleFor(w => w)
                .Must(w => _subscriptionPolicy.Allows(w.User, w.Strategy))
                .WithMessage("Your subscription plan does not allow this chunker strategy.");

            RuleFor(w => w.Name).NotEmpty().MaximumLength(100);
            RuleFor(w => w.Description).NotEmpty().MaximumLength(500);
            RuleFor(w => w.Strategy).IsInEnum();

            RuleFor(w => w.ConversationProvider).SetValidator(new ConversationProviderConfigValidator());
            RuleFor(w => w.EmbeddingProvider).SetValidator(new EmbeddingProviderConfigValidator());

            RuleFor(w => w.Settings.Threshold).InclusiveBetween(0, 1);
            RuleFor(w => w.Settings.MaxChunkSize).InclusiveBetween(100, 1200);
        }
    }
}