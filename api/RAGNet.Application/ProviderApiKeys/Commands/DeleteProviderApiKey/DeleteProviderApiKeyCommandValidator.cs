using FluentValidation;

namespace RAGNET.Application.ProviderApiKeys.Commands.DeleteProviderApiKey
{
    public class DeleteProviderApiKeyCommandValidator : AbstractValidator<DeleteProviderApiKeyCommand>
    {
        public DeleteProviderApiKeyCommandValidator()
        {
            RuleFor(p => p.ProviderApiKeyId).NotNull();
        }
    }
}