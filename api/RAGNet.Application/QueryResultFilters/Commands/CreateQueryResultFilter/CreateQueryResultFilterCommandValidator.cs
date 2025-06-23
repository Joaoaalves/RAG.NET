using FluentValidation;

namespace RAGNET.Application.QueryResultFilters.Commands.CreateQueryResultFilter
{
    public class CreateQueryResultFilterCommandValidator : AbstractValidator<CreateQueryResultFilterCommand>
    {
        public CreateQueryResultFilterCommandValidator()
        {
            RuleFor(qrf => qrf.Data.MaxItems).InclusiveBetween(1, 100);
        }
    }
}