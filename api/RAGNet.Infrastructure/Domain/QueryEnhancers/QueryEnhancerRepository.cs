using Microsoft.EntityFrameworkCore;
using RAGNET.Domain.QueryEnhancers;
using RAGNET.Infrastructure.Database;

namespace RAGNET.Infrastructure.Domain.QueryEnhancers
{
    public class QueryEnhancerRepository(ApplicationDbContext context) : IQueryEnhancerRepository
    {
        private readonly ApplicationDbContext _context = context ?? throw new ArgumentNullException(nameof(context));

        public async Task<QueryEnhancer> AddAsync(QueryEnhancer queryEnhancer)
        {
            await _context.QueryEnhancers.AddAsync(queryEnhancer);

            return queryEnhancer;
        }

        public Task DeleteAsync(QueryEnhancer queryEnhancer)
        {
            _context.QueryEnhancers.Remove(queryEnhancer);
            return Task.CompletedTask;
        }

        public async Task<QueryEnhancer?> GetByIdAsync(QueryEnhancerId id, string userId)
        {
            return await _context.QueryEnhancers
                .Include(qe => qe.Metas)
                .FirstOrDefaultAsync(qe => qe.Id == id && qe.UserId == userId);
        }

        public Task UpdateAsync(QueryEnhancer queryEnhancer)
        {
            _context.QueryEnhancers.Update(queryEnhancer);
            return Task.CompletedTask;
        }
    }
}