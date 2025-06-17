using RAGNET.Domain.SeedWork;
using RAGNET.Infrastructure.Database;
using Microsoft.EntityFrameworkCore;

namespace RAGNET.Infrastructure.Domain
{
    public class UnitOfWork(
        ApplicationDbContext ordersContext) : IUnitOfWork
    {
        private readonly ApplicationDbContext _ordersContext = ordersContext;

        public async Task<int> CommitAsync(CancellationToken cancellationToken = default)
        {
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
