using FluentValidation;

namespace RAGNET.Application.Queries.Commands.FilterQueryResult
{
    public class FilterQueryResultCommandValidator : AbstractValidator<FilterQueryResultCommand>
    {
        public FilterQueryResultCommandValidator()
        {
            RuleFor(fqr => fqr.Query).NotEmpty();
        }
    }
}