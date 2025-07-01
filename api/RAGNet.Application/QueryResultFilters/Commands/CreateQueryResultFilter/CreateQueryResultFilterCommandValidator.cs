using FluentValidation;
using RAGNET.Domain.SharedKernel.Plans.Policies;

namespace RAGNET.Application.QueryResultFilters.Commands.CreateQueryResultFilter
{
    public class CreateQueryResultFilterCommandValidator : AbstractValidator<CreateQueryResultFilterCommand>
    {

        private readonly SubscriptionPolicy _subscriptionPolicy;
        public CreateQueryResultFilterCommandValidator(SubscriptionPolicy subscriptionPolicy)
        {
            _subscriptionPolicy = subscriptionPolicy;

            RuleFor(qrf => qrf.Strategy).IsInEnum();

            RuleFor(qrf => qrf)
                .Must(qrf => _subscriptionPolicy.Allows(qrf.User, qrf.Strategy))
                .WithMessage("Your subscription plan does not allow this Query Result Filter Strategy");

            RuleFor(qrf => qrf.MaxItems).InclusiveBetween(1, 100);

        }
    }
}