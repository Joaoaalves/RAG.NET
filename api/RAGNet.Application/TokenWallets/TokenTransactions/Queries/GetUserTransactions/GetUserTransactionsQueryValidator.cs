using FluentValidation;

namespace RAGNET.Application.TokenWallets.TokenTransactions.Queries.GetUserTransactions
{
    public class GetUserTransactionsQueryValidator : AbstractValidator<GetUserTransactionsQuery>
    {
        public GetUserTransactionsQueryValidator()
        {
            RuleFor(q => q.Page).GreaterThan(0);
            RuleFor(q => q.PageSize).GreaterThan(0);

            RuleFor(t => t.Start).NotNull().Must(
                s => s <= DateTime.UtcNow && s != default
            ).WithMessage("Start date is required and must be valid");

            RuleFor(t => t.End).NotNull().Must(
                e => e <= DateTime.UtcNow && e != default
            ).WithMessage("End date is required and must be valid");

            RuleFor(t => t.End)
                .Must((query, end) => query.Start < end)
                .WithMessage("Start date must be before End date");

            RuleFor(t => t.End)
            .Must((query, end) =>
            {
                if (query.Start is DateTime start && end is DateTime endDate)
                {
                    return endDate <= start.AddMonths(6);
                }

                return true;
            })
            .WithMessage("The period between Start and End must not exceed 6 months")
            .When(query => query.Start != default && query.End != default);
        }
    }
}