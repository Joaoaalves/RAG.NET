using RAGNET.Domain.SeedWork;
using RAGNET.Infrastructure.Database;

namespace RAGNET.Infrastructure.Processing
{
    public class DomainEventsDispatcher(IMediator mediator, ApplicationDbContext context) : IDomainEventsDispatcher
    {
        private readonly IMediator _mediator = mediator ?? throw new ArgumentNullException(nameof(mediator));
        private readonly ApplicationDbContext _context = context ?? throw new ArgumentNullException(nameof(context));

        public async Task DispatchEventsAsync()
        {
            var domainEntities = _context.ChangeTracker
                .Entries<Entity>()
                .Where(e => e.Entity.DomainEvents != null && e.Entity.DomainEvents.Count != 0)
                .ToList();

            var domainEvents = domainEntities
                .SelectMany(e => e.Entity.DomainEvents!)
                .ToList();


            // Clear to avoid re-sending events
            domainEntities.ForEach(e => e.Entity.ClearDomainEvents());

            // Publish events via MediatR
            foreach (var domainEvent in domainEvents)
            {
                await _mediator.Publish(domainEvent);
            }
        }
    }
}
