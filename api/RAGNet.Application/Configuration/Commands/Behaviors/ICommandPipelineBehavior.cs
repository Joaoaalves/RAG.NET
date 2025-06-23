namespace RAGNET.Application.Configuration.Commands.Behaviors
{
    public interface ICommandPipelineBehavior<TCommand, TResult>
        where TCommand : ICommand<TResult>
    {
        Task<TResult> Handle(
            TCommand command,
            Func<TCommand, Task<TResult>> next,
            CancellationToken cancellationToken);
    }
}