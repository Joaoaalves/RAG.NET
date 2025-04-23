using Microsoft.EntityFrameworkCore;
using RAGNET.Domain.Entities;
using RAGNET.Domain.Repositories;
using RAGNET.Infrastructure.Data;

namespace RAGNET.Infrastructure.Repositories
{
    public class CallbackUrlRepository(ApplicationDbContext context) : Repository<CallbackUrl>(context), ICallbackUrlRepository
    {
        public async Task<List<string>> GetByWorkflowAsync(Guid workflowId)
        {
            var result = await _context.CallbackUrls.Where(c => c.WorkflowId == workflowId).ToListAsync();

            List<string> callbackUrls = [];

            foreach (var callbackUrl in result)
            {
                callbackUrls.Add(callbackUrl.Url);
            }

            return callbackUrls;
        }
    }
}