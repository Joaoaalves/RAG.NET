using FluentValidation;

namespace RAGNET.Application.ProviderApiKeys.Commands.UpdateProviderApiKey
{
    public class UpdateProviderApiKeyCommandValidator : AbstractValidator<UpdateProviderApiKeyCommand>
    {
        public UpdateProviderApiKeyCommandValidator()
        {
            RuleFor(p => p.ProviderApiKeyId).NotNull();
            RuleFor(p => p.ApiKey).MinimumLength(10);
        }
    }
}