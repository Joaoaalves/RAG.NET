using RAGNET.Domain.Documents;
using RAGNET.Domain.Entities;

namespace RAGNET.Domain.Services
{
    public interface IDocumentProcessingService
    {
        Task<DocumentExtractResult> ExtractTextAsync(Stream fileStream);
        Task<Document> CreateDocumentWithPagesAsync(string title, Guid workflowId, List<string> pages);
    }
}