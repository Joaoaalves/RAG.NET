using Microsoft.EntityFrameworkCore;
using RAGNET.Domain.Workflows;
using RAGNET.Infrastructure.Database;

namespace RAGNET.Infrastructure.Domain.Workflows;

public class WorkflowRepository(ApplicationDbContext context) : IWorkflowRepository
{
    private readonly ApplicationDbContext _context = context ?? throw new ArgumentNullException(nameof(context));

    public async Task<Workflow?> GetByIdAsync(WorkflowId id, string? userId)
    {
        return await _context.Workflows
            .Include(w => w.Chunker).ThenInclude(c => c.Metas)
            .Include(w => w.QueryEnhancers).ThenInclude(q => q!.Metas)
            .Include(w => w.QueryResultFilter)!.ThenInclude(f => f!.Metas)
            .Include(w => w.Rankers).ThenInclude(r => r.Metas)
            .Include(w => w.ConversationProviderConfig)
            .Include(w => w.Documents)
            .Include(w => w.EmbeddingProviderConfig)
            .Include(w => w.CallbackUrls)
            .FirstOrDefaultAsync(w => w.Id == id && (userId == null || w.UserId == userId));
    }

    public async Task<IEnumerable<Workflow>> GetAllAsync(string? userId)
    {
        return await _context.Workflows
            .Include(w => w.Chunker).ThenInclude(c => c!.Metas)
            .Include(w => w.QueryEnhancers).ThenInclude(q => q.Metas)
            .Include(w => w.QueryResultFilter)!.ThenInclude(f => f!.Metas)
            .Include(w => w.Rankers).ThenInclude(r => r.Metas)
            .Include(w => w.ConversationProviderConfig)
            .Include(w => w.Documents)
            .Include(w => w.EmbeddingProviderConfig)
            .Include(w => w.CallbackUrls)
            .Where(w => userId == null || w.UserId == userId)
            .ToListAsync();

    }

    public async Task<Workflow> AddAsync(Workflow workflow)
    {
        await _context.Workflows.AddAsync(workflow);
        return workflow;
    }

    public Task UpdateAsync(Workflow workflow, string? userId)
    {
        _context.Workflows.Update(workflow);
        return Task.CompletedTask;
    }

    public Task DeleteAsync(Workflow workflow, string? userId)
    {
        if (workflow.IsActive)
        {
            workflow.SetActivationState(false);
            _context.Workflows.Update(workflow);
            return Task.CompletedTask;
        }
        _context.Workflows.Remove(workflow);
        return Task.CompletedTask;

    }

    public async Task<Workflow?> GetByApiKey(string apiKey)
    {
        return await _context.Workflows
            .Include(w => w.Chunker).ThenInclude(c => c!.Metas)
            .Include(w => w.QueryEnhancers).ThenInclude(q => q.Metas)
            .Include(w => w.QueryResultFilter)!.ThenInclude(f => f!.Metas)
            .Include(w => w.Rankers).ThenInclude(r => r.Metas)
            .Include(w => w.ConversationProviderConfig)
            .Include(w => w.EmbeddingProviderConfig)
            .Include(w => w.Documents)
            .Include(w => w.CallbackUrls)
            .FirstOrDefaultAsync(w => w.ApiKey == apiKey);
    }

    public async Task<IEnumerable<Workflow>> GetUserWorkflows(string userId)
    {
        return await _context.Workflows
            .Include(w => w.Chunker).ThenInclude(c => c!.Metas)
            .Include(w => w.QueryEnhancers).ThenInclude(q => q.Metas)
            .Include(w => w.QueryResultFilter)!.ThenInclude(f => f!.Metas)
            .Include(w => w.Rankers).ThenInclude(r => r.Metas)
            .Include(w => w.ConversationProviderConfig)
            .Include(w => w.EmbeddingProviderConfig)
            .Include(w => w.CallbackUrls)
            .Include(w => w.Documents)
            .Where(w => w.UserId == userId)
            .ToListAsync();
    }

    public async Task UpdateByApiKey(Workflow workflow, string apiKey)
    {
        var existing = await _context.Workflows.FirstOrDefaultAsync(w => w.Id == workflow.Id);
        if (existing == null || existing.ApiKey != apiKey)
        {
            throw new UnauthorizedAccessException("Invalid workflow or API key.");
        }

        _context.Workflows.Update(workflow);
    }
}
