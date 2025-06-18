using RAGNET.Domain.SeedWork;

namespace RAGNET.Application.Configuration.Commands
{
    public interface ICommand : IRequest<Unit>
    {
        Guid Id { get; }
    }

    public interface ICommand<TResult> : IRequest<TResult>
    {
        Guid Id { get; }
    }

    public readonly struct Unit
    {
        public static readonly Unit Value = new();
    }
}
