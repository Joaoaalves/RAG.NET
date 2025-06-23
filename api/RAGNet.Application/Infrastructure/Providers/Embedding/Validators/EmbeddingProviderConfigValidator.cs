using FluentValidation;
using RAGNET.Application.Infrastructure.Providers.Embedding.DTOs;

namespace RAGNET.Application.Infrastructure.Providers.Embedding.Validators
{
    public class EmbeddingProviderConfigValidator : AbstractValidator<EmbeddingProviderConfigDTO>
    {
        public EmbeddingProviderConfigValidator()
        {
            RuleFor(emb => emb.ProviderId).IsInEnum();
            RuleFor(emb => emb.Model).NotNull().MinimumLength(5);
        }
    }
}