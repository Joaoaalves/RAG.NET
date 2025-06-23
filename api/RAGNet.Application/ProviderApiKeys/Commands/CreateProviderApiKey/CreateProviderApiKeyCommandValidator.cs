using FluentValidation;

namespace RAGNET.Application.ProviderApiKeys.Commands.CreateProviderApiKey
{
    public class CreateProviderApiKeyCommandValidator : AbstractValidator<CreateProviderApiKeyCommand>
    {
        public CreateProviderApiKeyCommandValidator()
        {
            RuleFor(p => p.Provider).IsInEnum();
            RuleFor(p => p.ApiKey).NotEmpty().MinimumLength(10);
        }
    }
}