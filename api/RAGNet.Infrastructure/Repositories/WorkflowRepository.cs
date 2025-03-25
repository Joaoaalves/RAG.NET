
using Microsoft.EntityFrameworkCore;
using RAGNET.Domain.Entities;
using RAGNET.Domain.Repositories;
using RAGNET.Infrastructure.Data;

namespace RAGNET.Infrastructure.Repositories
{
    public class WorkflowRepository(ApplicationDbContext context) : Repository<Workflow>(context), IWorkflowRepository
    {
        public async Task<Workflow?> GetWithRelationsAsync(Guid id)
        {
            return await _context.Workflows
                .Include(w => w.Chunkers)
                    .ThenInclude(c => c.Metas)
                .Include(w => w.QueryEnhancers)
                    .ThenInclude(q => q.Metas)
                .Include(w => w.Filters)
                    .ThenInclude(f => f.Metas)
                .Include(w => w.Rankers)
                    .ThenInclude(r => r.Metas)
                .FirstOrDefaultAsync(w => w.Id == id);
        }

        public async Task<IEnumerable<Workflow>> GetUserWorkflows(string userId)
        {
            return await _context.Workflows.Include(w => w.Chunkers)
                    .ThenInclude(c => c.Metas)
                .Include(w => w.QueryEnhancers)
                    .ThenInclude(q => q.Metas)
                .Include(w => w.Filters)
                    .ThenInclude(f => f.Metas)
                .Include(w => w.Rankers)
                    .ThenInclude(r => r.Metas).Where(w => w.UserId == userId).ToListAsync();
        }
    }
}
