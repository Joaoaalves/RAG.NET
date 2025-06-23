using Microsoft.EntityFrameworkCore;
using RAGNET.Domain.Documents;
using RAGNET.Domain.Documents.Pages;
using RAGNET.Infrastructure.Database;

namespace RAGNET.Infrastructure.Domain.Documents.Pages
{
    public class PageRepository(ApplicationDbContext context) : IPageRepository
    {
        private readonly ApplicationDbContext _context = context;

        public async Task<List<Page>> GetManyByDocumentId(DocumentId documentId)
        {
            return await _context.Pages
                .Where(p => p.DocumentId == documentId)
                .Include(p => p.Chunks)
                .ToListAsync();
        }

        public async Task<List<Page>> GetManyAsync(PageId[] pageIds)
        {
            return await _context.Pages
                .Where(p => pageIds.Contains(p.Id))
                .Include(p => p.Chunks)
                .ToListAsync();
        }

        public async Task<Page> AddAsync(Page page)
        {
            await _context.Pages.AddAsync(page);
            return page;
        }
    }
}