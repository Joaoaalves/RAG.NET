using RAGNET.Domain.SeedWork;
using RAGNET.Infrastructure.Database;
using Microsoft.EntityFrameworkCore;
using RAGNET.Infrastructure.Processing;

namespace RAGNET.Infrastructure.Domain
{
    public class UnitOfWork(
        ApplicationDbContext ordersContext,
        IDomainEventsDispatcher domainEventsDispatcher) : IUnitOfWork
    {
        private readonly ApplicationDbContext _ordersContext = ordersContext;
        private readonly IDomainEventsDispatcher _domainEventsDispatcher = domainEventsDispatcher;

        public async Task<int> CommitAsync(CancellationToken cancellationToken = default)
        {
            await _domainEventsDispatcher.DispatchEventsAsync();
            return await _ordersContext.SaveChangesAsync(cancellationToken);
        }

        public Task RevertAsync()
        {
            foreach (var entry in _ordersContext.ChangeTracker.Entries())
            {
                switch (entry.State)
                {
                    case EntityState.Modified:
                    case EntityState.Deleted:
                        entry.State = EntityState.Unchanged;
                        break;

                    case EntityState.Added:
                        entry.State = EntityState.Detached;
                        break;
                }
            }
            return Task.CompletedTask;
        }
    }
}
