using FluentValidation;

namespace RAGNET.Application.QueryEnhancers.Commands.EnhanceQuery
{
    public class EnhanceQueryCommandValidator : AbstractValidator<EnhanceQueryCommand>
    {
        public EnhanceQueryCommandValidator()
        {
            RuleFor(qe => qe.QueryDTO.Query).NotEmpty();
            RuleFor(qe => qe.QueryDTO.TopK).InclusiveBetween(1, 10);
            RuleFor(qe => qe.QueryDTO.ParentChild).NotNull();
            RuleFor(qe => qe.QueryDTO.NormalizeScore).NotNull();

            RuleFor(qe => qe.QueryDTO.MinNormalizedScore)
                .InclusiveBetween(0, 1)
                .When(qe => qe.QueryDTO.NormalizeScore);

            RuleFor(qe => qe.QueryDTO.MinScore)
                .InclusiveBetween(0, 1)
                .When(qe => !qe.QueryDTO.NormalizeScore);
        }
    }
}