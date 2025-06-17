using Microsoft.EntityFrameworkCore;
using RAGNET.Domain.Documents;
using RAGNET.Domain.Workflows;
using RAGNET.Infrastructure.Database;

namespace RAGNET.Infrastructure.Domain.Documents
{
    public class DocumentRepository(ApplicationDbContext context) : IDocumentRepository
    {
        private readonly ApplicationDbContext _context = context ?? throw new ArgumentNullException(nameof(context));

        public async Task<Document> AddAsync(Document document)
        {
            await _context.Documents.AddAsync(document);

            return document;
        }

        public Task DeleteAsync(Document document)
        {
            ArgumentNullException.ThrowIfNull(document, nameof(document));
            _context.Documents.Remove(document);

            return Task.CompletedTask;
        }

        public Task<Document?> GetByIdAsync(Guid id, Guid workflowId)
        {
            return _context.Documents
                .Include(d => d.Pages)
                .FirstOrDefaultAsync(d => d.Id == id && d.WorkflowId == workflowId);
        }

        public Task UpdateAsync(Document document)
        {
            ArgumentNullException.ThrowIfNull(document, nameof(document));
            _context.Documents.Update(document);

            return Task.CompletedTask;
        }
    }
}