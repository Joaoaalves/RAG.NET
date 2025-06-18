using RAGNET.Application.Configuration.Commands;

namespace RAGNET.Application.Configuration.Processing
{
    public interface ICommandsScheduler
    {
        Task EnqueueAsync<T>(ICommand<T> command);
    }
}