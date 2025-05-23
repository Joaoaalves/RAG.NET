using RAGNET.Domain.Entities;
using Microsoft.AspNetCore.Http;

namespace RAGNET.Domain.Services
{
    public interface IDocumentProcessingService
    {
        Task<DocumentExtractResult> ExtractTextAsync(Stream fileStream);
        Task<Document> CreateDocumentWithPagesAsync(string title, Guid workflowId, List<string> pages);
    }
}