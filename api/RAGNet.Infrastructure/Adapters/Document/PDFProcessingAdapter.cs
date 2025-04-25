using RAGNET.Domain.Services;
using UglyToad.PdfPig;
using Microsoft.AspNetCore.Http;
using RAGNET.Domain.Entities;
using RAGNET.Domain.Repositories;

namespace RAGNET.Infrastructure.Adapters.Document
{
    public class PDFProcessingAdapter(IDocumentRepository documentRepository, IPageRepository pageRepository) : IDocumentProcessingService
    {
        private readonly IDocumentRepository _documentRepository = documentRepository;
        private readonly IPageRepository _pageRepository = pageRepository;

        public async Task<Domain.Entities.Document> CreateDocumentWithPagesAsync(string title, Guid workflowId, List<string> pages)
        {
            var document = await _documentRepository.AddAsync(new Domain.Entities.Document
            {
                Title = title,
                WorkflowId = workflowId,
                Pages = []
            });


            foreach (var pageText in pages)
            {
                await _pageRepository.AddAsync(new Page
                {
                    Text = pageText.Trim(),
                    DocumentId = document.Id,
                });
            }

            return document;
        }

        public async Task<DocumentExtractResult> ExtractTextAsync(Stream fileStream)
        {
            // Open the PDF stream
            var result = new DocumentExtractResult();

            // Wraps the result in Task.FromResult.
            using (var pdf = PdfDocument.Open(fileStream))
            {
                foreach (var pageText in pdf.GetPages())
                {
                    result.Pages.Add(pageText.Text);
                }
            }

            return await Task.FromResult(result);
        }
    }
}
