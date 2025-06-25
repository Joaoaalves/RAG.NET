using FluentValidation;

namespace RAGNET.Application.TokenWallets.Queries.GetUserDailyTransactions
{
    public class GetUserDailyTransactionsQueryValidator : AbstractValidator<GetUserDailyTransactionsQuery>
    {
        private static readonly TimeSpan SixMonthsApproximation = TimeSpan.FromDays(6 * 30);
        public GetUserDailyTransactionsQueryValidator()
        {
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
                DateTime maxAllowedEnd = query.Start.AddMonths(6);
                return end <= maxAllowedEnd;
            })
            .WithMessage("The period between Start and End must not exceed 6 months")
            .When(query => query.Start != default && query.End != default);
        }
    }
}