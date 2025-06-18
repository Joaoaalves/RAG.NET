using Microsoft.Extensions.DependencyInjection;
using RAGNET.Application.Configuration.Commands;
using RAGNET.Domain.SeedWork;

namespace RAGNET.Infrastructure.Processing
{
    public class CommandsExecutor(IServiceScopeFactory scopeFactory)
    {
        private readonly IServiceScopeFactory _scopeFactory = scopeFactory;

        public async Task Execute(ICommand command)
        {
            using var scope = _scopeFactory.CreateScope();
            var mediator = scope.ServiceProvider.GetRequiredService<IMediator>();
            await mediator.Send(command);
        }

        public async Task<TResult> Execute<TResult>(ICommand<TResult> command)
        {
            using var scope = _scopeFactory.CreateScope();
            var mediator = scope.ServiceProvider.GetRequiredService<IMediator>();
            return await mediator.Send(command);
        }
    }
}