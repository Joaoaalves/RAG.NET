using FluentValidation;
using RAGNET.Domain.SharedKernel.Plans.Policies;

namespace RAGNET.Application.QueryEnhancers.Commands.CreateQueryEnhancer
{
    public class CreateQueryEnhancerCommandValidator : AbstractValidator<CreateQueryEnhancerCommand>
    {
        private readonly SubscriptionPolicy _subscriptionPolicy;
        public CreateQueryEnhancerCommandValidator(SubscriptionPolicy subscriptionPolicy)
        {

            _subscriptionPolicy = subscriptionPolicy;

            RuleFor(qe => qe.Type).IsInEnum();

            RuleFor(qe => qe)
                .Must(qe => _subscriptionPolicy.Allows(qe.User, qe.Type))
                .WithMessage("Your subscription plan does not allow this Query Enhancer Strategy");

            RuleFor(qe => qe.Prompt).NotEmpty().MinimumLength(20).When(qe => qe.Type == Domain.QueryEnhancers.QueryEnhancerStrategy.AUTO_QUERY);
            RuleFor(qe => qe.MaxQueries).InclusiveBetween(1, 10);
        }
    }
}