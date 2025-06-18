
namespace RAGNET.Application.Configuration.Commands
{
    public abstract class CommandBase : ICommand
    {
        public Guid Id { get; } = Guid.NewGuid();

        protected CommandBase()
        {
        }

        protected CommandBase(Guid id)
        {
            Id = id;
        }
    }

    public abstract class CommandBase<TResult> : ICommand<TResult>
    {
        public Guid Id { get; } = Guid.NewGuid();

        protected CommandBase()
        {
        }

        protected CommandBase(Guid id)
        {
            Id = id;
        }
    }
}