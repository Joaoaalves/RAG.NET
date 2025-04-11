using Microsoft.AspNetCore.Http;
using RAGNET.Domain.Entities;

namespace RAGNET.Domain.Services
{
    public class PDFExtractResult
    {
        public string DocumentTitle { get; set; } = String.Empty;
        public List<string> Pages { get; set; } = [];
    }

    public interface IPDFProcessingService
    {
        Task<PDFExtractResult> ExtractTextAsync(IFormFile file);
        Task<Document> CreateDocumentWithPagesAsync(string title, Guid workflowId, List<string> pages);
    }
}