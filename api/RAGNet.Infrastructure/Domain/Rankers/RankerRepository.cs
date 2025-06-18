using Microsoft.EntityFrameworkCore;
using RAGNET.Domain.Rankers;
using RAGNET.Infrastructure.Database;

namespace RAGNET.Infrastructure.Domain.Rankers
{
    public class RankerRepository(ApplicationDbContext context) : IRankerRepository
    {
        private readonly ApplicationDbContext _dbContext = context ?? throw new ArgumentNullException(nameof(context));

        public async Task<Ranker?> GetByIdAsync(RankerId id, string userId)
        {
            return await _dbContext.Rankers.Include(r => r.Metas)
                .FirstOrDefaultAsync(r => r.Id == id && r.UserId == userId);
        }

        public async Task<Ranker?> AddAsync(Ranker ranker)
        {
            await _dbContext.Rankers.AddAsync(ranker);
            return ranker;
        }

        public async Task UpdateAsync(Ranker ranker)
        {
            _dbContext.Rankers.Update(ranker);
            await Task.CompletedTask;
        }

        public async Task DeleteAsync(Ranker ranker)
        {
            _dbContext.Rankers.Remove(ranker);
            await Task.CompletedTask;
        }
    }
}