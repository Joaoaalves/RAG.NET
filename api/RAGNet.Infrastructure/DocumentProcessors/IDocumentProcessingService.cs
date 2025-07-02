using RAGNET.Application.Workflows.Commands.EnqueueEmbeddingJob.DocumentProcessors;
using RAGNET.Domain.Documents;

namespace RAGNET.Infrastructure.DocumentProcessors
{
    public interface IDocumentProcessingService
    {
        Task<DocumentExtractResult> ExtractTextAsync(Stream fileStream);
        Task<Document> CreateDocumentWithPagesAsync(Document document, List<string> pages);
    }
}