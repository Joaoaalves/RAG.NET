using FluentValidation;

namespace RAGNET.Application.QueryEnhancers.Commands.CreateQueryEnhancer
{
    public class CreateQueryEnhancerCommandValidator : AbstractValidator<CreateQueryEnhancerCommand>
    {
        public CreateQueryEnhancerCommandValidator()
        {
            RuleFor(qe => qe.Type).IsInEnum();
            RuleFor(qe => qe.Prompt).NotEmpty().MinimumLength(20).When(qe => qe.Type == Domain.QueryEnhancers.QueryEnhancerStrategy.AUTO_QUERY);
            RuleFor(qe => qe.MaxQueries).InclusiveBetween(1, 10);
        }
    }
}