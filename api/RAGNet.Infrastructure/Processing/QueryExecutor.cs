using Microsoft.Extensions.DependencyInjection;
using RAGNET.Application.Configuration.Queries;
using RAGNET.Domain.SeedWork;

namespace RAGNET.Infrastructure.Processing
{
    public class QueriesExecutor(IServiceScopeFactory scopeFactory)
    {
        private readonly IServiceScopeFactory _scopeFactory = scopeFactory;

        public async Task<TResult> Execute<TResult>(IQuery<TResult> query)
        {
            using var scope = _scopeFactory.CreateScope();
            var mediator = scope.ServiceProvider.GetRequiredService<IMediator>();
            return await mediator.Send(query);
        }
    }
}