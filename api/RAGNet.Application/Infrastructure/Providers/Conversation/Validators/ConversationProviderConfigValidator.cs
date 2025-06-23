using FluentValidation;
using RAGNET.Application.Infrastructure.Providers.Conversation.DTOs;

namespace RAGNET.Application.Infrastructure.Providers.Conversation.Validators
{
    public class ConversationProviderConfigValidator : AbstractValidator<ConversationProviderConfigDTO>
    {
        public ConversationProviderConfigValidator()
        {
            RuleFor(emb => emb.ProviderId).IsInEnum();
            RuleFor(emb => emb.Model).NotNull().MinimumLength(5);
        }
    }
}