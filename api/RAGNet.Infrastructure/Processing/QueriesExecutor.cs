using Microsoft.Extensions.DependencyInjection;
using RAGNET.Application.Configuration.Queries;
using RAGNET.Domain.SeedWork;

namespace RAGNET.Infrastructure.Processing
{
    public class QueriesExecutor(IServiceProvider provider)
    {
        private readonly IServiceProvider _provider = provider;

        public async Task<TResult> Execute<TResult>(IQuery<TResult> query)
        {
            using var scope = _provider.CreateScope();
            var sp = scope.ServiceProvider;

            var mediator = sp.GetRequiredService<IMediator>();

            var behaviors = sp
                .GetServices(typeof(IRequestPipelineBehavior<,>).MakeGenericType(query.GetType(), typeof(TResult)))
                .Cast<dynamic>()
                .Reverse()
                .ToList();

            Func<IQuery<TResult>, Task<TResult>> handler = (q) => mediator.Send(q);

            foreach (var behavior in behaviors)
            {
                var next = handler;

                handler = (q) =>
                {
                    Task<TResult> nextDelegate() => next(q);
                    return behavior.Handle((dynamic)q, (Func<Task<TResult>>)nextDelegate, CancellationToken.None);
                };
            }

            return await handler(query);
        }
    }
}
