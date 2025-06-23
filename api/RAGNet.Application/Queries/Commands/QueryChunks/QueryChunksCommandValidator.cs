using FluentValidation;

namespace RAGNET.Application.Queries.Commands.QueryChunks
{
    public class QueryChunksCommandValidator : AbstractValidator<QueryChunksCommand>
    {
        public QueryChunksCommandValidator()
        {
            RuleFor(qc => qc.QueryDTO.Query).NotEmpty();
            RuleFor(qc => qc.QueryDTO.TopK).InclusiveBetween(1, 10);
            RuleFor(qc => qc.QueryDTO.ParentChild).NotNull();
            RuleFor(qc => qc.QueryDTO.NormalizeScore).NotNull();

            RuleFor(qc => qc.QueryDTO.MinNormalizedScore)
                .InclusiveBetween(0, 1)
                .When(qc => qc.QueryDTO.NormalizeScore);

            RuleFor(qc => qc.QueryDTO.MinScore)
                .InclusiveBetween(0, 1)
                .When(qc => !qc.QueryDTO.NormalizeScore);
        }
    }
}