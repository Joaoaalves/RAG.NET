using RAGNET.Domain.Documents;

namespace RAGNET.Infrastructure.DocumentProcessors
{
    public interface IDocumentProcessingService
    {
        Task<DocumentExtractResult> ExtractTextAsync(Stream fileStream);
        Task<Document> CreateDocumentWithPagesAsync(string title, Guid workflowId, List<string> pages);
    }
}