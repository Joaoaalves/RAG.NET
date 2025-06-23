
namespace RAGNET.Application.Configuration.Commands
{
    public class BaseCommand<TResult> : ICommand<TResult>
    {
        public Guid Id { get; } = Guid.NewGuid();
    }
}