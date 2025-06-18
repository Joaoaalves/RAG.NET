using RAGNET.Domain.Documents;
using RAGNET.Domain.Workflows;

namespace RAGNET.Infrastructure.DocumentProcessors
{
    public interface IDocumentProcessingService
    {
        Task<DocumentExtractResult> ExtractTextAsync(Stream fileStream);
        Task<Document> CreateDocumentWithPagesAsync(string title, WorkflowId workflowId, List<string> pages);
    }
}