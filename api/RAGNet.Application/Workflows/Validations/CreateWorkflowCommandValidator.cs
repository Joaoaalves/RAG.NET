using FluentValidation;
using RAGNET.Application.Workflows.CreateWorkflow;

namespace RAGNET.Application.Workflows.Validations
{

    public class CreateWorkflowCommandValidator : AbstractValidator<CreateWorkflowCommand>
    {
        public CreateWorkflowCommandValidator()
        {
            RuleFor(c => c.Dto.Name).NotEmpty().MaximumLength(100);
            RuleFor(c => c.Dto.EmbeddingProvider).NotNull();
            RuleFor(c => c.Dto.ConversationProvider).NotNull();
        }

    }

}