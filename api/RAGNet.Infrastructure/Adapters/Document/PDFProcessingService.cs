using RAGNET.Domain.Services;
using UglyToad.PdfPig;
using Microsoft.AspNetCore.Http;
using RAGNET.Domain.Entities;
using RAGNET.Domain.Repositories;

namespace RAGNET.Infrastructure.Adapters.Document
{
    public class PdfProcessingAdapter(IDocumentRepository documentRepository, IPageRepository pageRepository) : IPDFProcessingService
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

        public async Task<PDFExtractResult> ExtractTextAsync(IFormFile file)
        {
            // Open the PDF stream
            using var stream = file.OpenReadStream();
            var result = new PDFExtractResult();

            // Wraps the result in Task.FromResult.
            using (var pdf = PdfDocument.Open(stream))
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
