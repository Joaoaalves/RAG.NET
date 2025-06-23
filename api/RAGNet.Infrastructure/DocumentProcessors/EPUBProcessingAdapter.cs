using HtmlAgilityPack;
using VersOne.Epub;
using System.Text;

using RAGNET.Domain.Documents;
using RAGNET.Domain.Documents.Pages;
using RAGNET.Domain.SeedWork;
using RAGNET.Domain.Workflows;

namespace RAGNET.Infrastructure.DocumentProcessors
{
    public class EpubProcessingAdapter(
        IDocumentRepository documentRepository,
        IUnitOfWork unitOfWork) : IDocumentProcessingService
    {
        private readonly IDocumentRepository _documentRepository = documentRepository;
        private readonly IUnitOfWork _unitOfWork = unitOfWork;
        public async Task<DocumentExtractResult> ExtractTextAsync(Stream fileStream)
        {
            var result = new DocumentExtractResult();

            EpubBook epubBook = await EpubReader.ReadBookAsync(fileStream);

            result.DocumentTitle = epubBook.Title ?? "Untitled";

            foreach (var contentFile in epubBook.ReadingOrder)
            {
                if (contentFile == null || string.IsNullOrWhiteSpace(contentFile.Content))
                    continue;

                // Extract only the desired HTML elements and ignore unwanted tags
                var cleanedText = ExtractCleanTextFromHtml(contentFile.Content);

                if (!string.IsNullOrWhiteSpace(cleanedText))
                {
                    result.Pages.Add(cleanedText);
                }
            }

            return result;
        }

        public async Task<Document> CreateDocumentWithPagesAsync(string title, WorkflowId workflowId, List<string> pages)
        {
            var document = Document.Create(new Text(title), workflowId);

            foreach (var pageText in pages)
            {
                var text = new Text(pageText);

                var page = Page.Create(
                    text, document.Id
                );

                // Associate the page with the document
                document.AddPage(page);
            }

            await _documentRepository.AddAsync(document);

            // Commit the changes to the database
            await _unitOfWork.CommitAsync();

            return document;
        }

        private string ExtractCleanTextFromHtml(string html)
        {
            var htmlDoc = new HtmlDocument();
            htmlDoc.LoadHtml(html);

            // Remove undesired tags
            string[] tagsToRemove = ["title", "meta", "link", "style", "img", "script", "nav"];
            foreach (var tag in tagsToRemove)
            {
                var nodes = htmlDoc.DocumentNode.SelectNodes($"//{tag}");
                if (nodes != null)
                {
                    foreach (var node in nodes)
                        node.Remove();
                }
            }

            var sb = new StringBuilder();

            // Extract <h3> and <p> content
            var h3Nodes = htmlDoc.DocumentNode.SelectNodes("//h3");
            if (h3Nodes != null)
            {
                foreach (var h3 in h3Nodes)
                    sb.AppendLine(h3.InnerText.Trim());
            }

            var pNodes = htmlDoc.DocumentNode.SelectNodes("//p");
            if (pNodes != null)
            {
                foreach (var p in pNodes)
                    sb.AppendLine(p.InnerText.Trim());
            }

            return sb.ToString();
        }
    }
}
