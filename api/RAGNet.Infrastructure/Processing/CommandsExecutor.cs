using Microsoft.Extensions.DependencyInjection;
using RAGNET.Application.Configuration.Commands;
using RAGNET.Application.Configuration.Commands.Behaviors;
using RAGNET.Domain.SeedWork;

namespace RAGNET.Infrastructure.Processing
{
    public class CommandsExecutor(IServiceProvider provider)
    {
        private readonly IServiceProvider _provider = provider;

        public async Task<TResult> Execute<TResult>(ICommand<TResult> command)
        {
            using var scope = _provider.CreateScope();

            var mediator = scope.ServiceProvider.GetRequiredService<IMediator>();

            // Search specific behaviors
            var behaviorsType = typeof(ICommandPipelineBehavior<,>).MakeGenericType(command.GetType(), typeof(TResult));
            var behaviors = scope.ServiceProvider.GetServices(behaviorsType)
                .Cast<dynamic>()
                .Reverse()
                .ToList();

            Task<TResult> handlerDelegate(dynamic cmd) => mediator.Send((ICommand<TResult>)cmd);

            var pipeline = behaviors.Aggregate(
            handlerDelegate,
                (next, behavior) => (cmd) => behavior.Handle(cmd, next, CancellationToken.None)
            );

            return await pipeline(command);
        }

        public async Task Execute(ICommand command)
        {
            using var scope = _provider.CreateScope();

            var mediator = scope.ServiceProvider.GetRequiredService<IMediator>();
            await mediator.Send(command);
        }
    }
}
