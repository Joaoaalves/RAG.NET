using FluentValidation;

namespace RAGNET.Application.QueryResultFilters.Commands.UpdateQueryResultFilter
{
    public class UpdateQueryResultFilterCommandValidator : AbstractValidator<UpdateQueryResultFilterCommand>
    {
        public UpdateQueryResultFilterCommandValidator()
        {
            RuleFor(qe => qe.Data.MaxItems).InclusiveBetween(1, 100);
            RuleFor(qe => qe.Data.IsEnabled).NotNull().When(qe => qe.Data.IsEnabled.HasValue);
            RuleFor(qe => qe.Strategy).IsInEnum();
        }
    }
}