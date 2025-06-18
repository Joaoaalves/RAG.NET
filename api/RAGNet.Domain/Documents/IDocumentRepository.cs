using RAGNET.Domain.Workflows;

namespace RAGNET.Domain.Documents
{
    public interface IDocumentRepository
    {
        Task<Document> AddAsync(Document document);
        Task<Document?> GetByIdAsync(DocumentId id, WorkflowId workflowId);
        Task UpdateAsync(Document document);
        Task DeleteAsync(Document document);
    }
}