
using Microsoft.EntityFrameworkCore;
using RAGNET.Domain.Entities;
using RAGNET.Domain.Repositories;
using RAGNET.Infrastructure.Data;

namespace RAGNET.Infrastructure.Repositories
{
    public class WorkflowRepository(ApplicationDbContext context) : Repository<Workflow>(context), IWorkflowRepository
    {
        public async Task<Workflow?> GetWithRelationsAsync(Guid id, string userId)
        {
            return await _context.Workflows
                .Include(w => w.Chunker)
                    .ThenInclude(c => c != null ? c.Metas : null)
                .Include(w => w.QueryEnhancers)
                    .ThenInclude(q => q.Metas)
                .Include(w => w.Filter)
                    .ThenInclude(f => f!.Metas)
                .Include(w => w.Rankers)
                    .ThenInclude(r => r.Metas)
                .Include(w => w.ConversationProviderConfig)
                .Include(w => w.EmbeddingProviderConfig)
                .FirstOrDefaultAsync(w => w.Id == id && w.UserId == userId);
        }

        public async Task<Workflow?> GetWithRelationsByApiKey(string apiKey)
        {
            return await _context.Workflows
                .Include(w => w.Chunker)
                    .ThenInclude(c => c != null ? c.Metas : null)
                .Include(w => w.QueryEnhancers)
                    .ThenInclude(q => q.Metas)
                .Include(w => w.Filter)
                    .ThenInclude(f => f!.Metas)
                .Include(w => w.Rankers)
                    .ThenInclude(r => r.Metas)
                .Include(w => w.ConversationProviderConfig)
                .Include(w => w.EmbeddingProviderConfig)
                .FirstOrDefaultAsync(w => w.ApiKey == apiKey);
        }

        public async Task<IEnumerable<Workflow>> GetUserWorkflows(string userId)
        {
            return await _context.Workflows.Include(w => w.Chunker)
                    .ThenInclude(c => c != null ? c.Metas : null)
                .Include(w => w.QueryEnhancers)
                    .ThenInclude(q => q.Metas)
                .Include(w => w.Filter)
                    .ThenInclude(f => f!.Metas)
                .Include(w => w.Rankers)
                    .ThenInclude(r => r.Metas).Where(w => w.UserId == userId)
                .Include(w => w.ConversationProviderConfig)
                .Include(w => w.EmbeddingProviderConfig)
                    .ToListAsync();
        }

        public async Task UpdateByApiKey(Workflow workflow, string apiKey)
        {
            var w = await _context.Workflows.FindAsync(workflow.Id) ?? throw new UnauthorizedAccessException("Workflow not found");
            if (w.ApiKey != apiKey)
            {
                throw new UnauthorizedAccessException("Wrong apikey");
            }

            _context.Update(workflow);
            await _context.SaveChangesAsync();

        }
    }
}
