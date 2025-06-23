using FluentValidation;

namespace RAGNET.Application.QueryEnhancers.Commands.UpdateQueryEnhancer
{
    public class UpdateQueryEnhancerCommandValidator : AbstractValidator<UpdateQueryEnhancerCommand>
    {
        public UpdateQueryEnhancerCommandValidator()
        {
            RuleFor(qe => qe.Strategy).IsInEnum();
            RuleFor(qe => qe.IsEnabled).NotNull().When(qe => qe.IsEnabled.HasValue);
            RuleFor(qe => qe.MaxQueries).InclusiveBetween(1, 10);
            RuleFor(qe => qe.Guidance).MinimumLength(20).When(qe => qe.Strategy == Domain.QueryEnhancers.QueryEnhancerStrategy.AUTO_QUERY);
        }
    }
}