using UglyToad.PdfPig;

using RAGNET.Domain.Documents;
using RAGNET.Domain.Documents.Pages;
using RAGNET.Domain.SeedWork;
using RAGNET.Domain.Workflows; // Should use the Document from here

namespace RAGNET.Infrastructure.DocumentProcessors
{
    public class PDFProcessingAdapter(IDocumentRepository documentRepository, IUnitOfWork unitOfWork) : IDocumentProcessingService
    {
        private readonly IDocumentRepository _documentRepository = documentRepository;
        private readonly IUnitOfWork _unitOfWork = unitOfWork;
        public async Task<Document> CreateDocumentWithPagesAsync(string title, WorkflowId workflowId, List<string> pages)
        {
            var document = Document.Create(new Text(title), workflowId);

            foreach (var pageText in pages)
            {
                try
                {
                    var text = new Text(pageText);

                    var page = Page.Create(
                        text, document.Id
                    );

                    // Associate the page with the document
                    document.AddPage(page);
                }
                catch (BusinessRuleValidationException)
                {
                    Console.WriteLine("Empty chunk detected. Jumping to the next.");
                }
                catch (Exception)
                {
                    Console.WriteLine("Unknow Error occurred!");
                }
            }

            await _documentRepository.AddAsync(document);

            // Commit the changes to the database
            await _unitOfWork.CommitAsync();

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
