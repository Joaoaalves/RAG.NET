using Microsoft.EntityFrameworkCore;
using RAGNET.Domain.Workflows.CallbackUrls;
using RAGNET.Infrastructure.Database;

namespace RAGNET.Infrastructure.Domain.Workflows.CallbackUrls
{
    public class CallbackUrlRepository(ApplicationDbContext context) : ICallbackUrlRepository
    {
        private readonly ApplicationDbContext _context = context ?? throw new ArgumentNullException(nameof(context));

        public async Task<CallbackUrl> AddAsync(CallbackUrl callbackUrl)
        {
            ArgumentNullException.ThrowIfNull(callbackUrl);

            await _context.CallbackUrls.AddAsync(callbackUrl);
            return callbackUrl;
        }

        public Task DeleteAsync(CallbackUrl callbackUrl, Guid workflowId)
        {
            ArgumentNullException.ThrowIfNull(callbackUrl);

            _context.CallbackUrls.Remove(callbackUrl);
            return Task.CompletedTask;
        }

        public async Task<IEnumerable<CallbackUrl>> GetAllAsync(Guid workflowId)
        {
            return await _context.CallbackUrls
                .Where(c => c.WorkflowId == workflowId)
                .ToListAsync();
        }

        public async Task<CallbackUrl?> GetByIdAsync(Guid id, Guid workflowId)
        {
            if (id == Guid.Empty) throw new ArgumentException("Invalid ID", nameof(id));

            return await _context.CallbackUrls
                .FirstOrDefaultAsync(c => c.Id == id && (c.WorkflowId == workflowId));
        }

        public async Task<List<CallbackUrl>> GetByWorkflowIdAsync(Guid workflowId)
        {
            if (workflowId == Guid.Empty) throw new ArgumentException("Invalid workflow ID", nameof(workflowId));

            return await _context.CallbackUrls
                .Where(c => c.WorkflowId == workflowId)
                .ToListAsync();
        }

        public Task UpdateAsync(CallbackUrl entity, Guid workflowId)
        {
            ArgumentNullException.ThrowIfNull(entity);

            _context.CallbackUrls.Update(entity);
            return Task.CompletedTask;
        }

    }
}