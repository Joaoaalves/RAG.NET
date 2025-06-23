namespace RAGNET.Application.Configuration.Commands.Behaviors
{
    public class LoggingBehavior<TCommand, TResult>
        : ICommandPipelineBehavior<TCommand, TResult>
        where TCommand : ICommand<TResult>
    {
        public async Task<TResult> Handle(
            TCommand command,
            Func<TCommand, Task<TResult>> next,
            CancellationToken cancellationToken)
        {
            try
            {
                return await next(command);
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error on command: {typeof(TCommand).Name}: {ex.Message}");
                throw;
            }
        }
    }

}