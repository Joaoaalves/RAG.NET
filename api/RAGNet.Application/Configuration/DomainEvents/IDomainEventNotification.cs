using RAGNET.Domain.SeedWork;

namespace RAGNET.Application.Configuration.DomainEvents
{
    public interface IDomainEventNotification<TEventType> : IDomainEventNotification
    {
        TEventType DomainEvent { get; }
    }

    public interface IDomainEventNotification : INotification
    {
        Guid Id { get; }
    }
}