using Microsoft.EntityFrameworkCore;
using RAGNET.Domain.Entities;
using RAGNET.Domain.Repositories;
using RAGNET.Infrastructure.Data;

namespace RAGNET.Infrastructure.Repositories
{
    public class PageRepository(ApplicationDbContext context) : Repository<Page>(context), IPageRepository
    {
        public async Task<List<Page>> GetMany(Guid[] ids)
        {
            return await _context.Pages.Where(c => ids.Contains(c.Id)).ToListAsync();
        }
    }
}