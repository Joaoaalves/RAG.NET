using HtmlAgilityPack;
using Microsoft.AspNetCore.Http;
using RAGNET.Domain.Entities;
using RAGNET.Domain.Repositories;
using RAGNET.Domain.Services;
using VersOne.Epub;
using System.Text;
using RAGNET.Domain.Documents;

namespace RAGNET.Infrastructure.Adapters.Documents
{
    public class EpubProcessingAdapter(
        IDocumentRepository documentRepository,
        IPageRepository pageRepository) : IDocumentProcessingService
    {
        private readonly IDocumentRepository _documentRepository = documentRepository;
        private readonly IPageRepository _pageRepository = pageRepository;

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

        public async Task<Document> CreateDocumentWithPagesAsync(string title, Guid workflowId, List<string> pages)
        {
            var document = await _documentRepository.AddAsync(new Document
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
                    DocumentId = document.Id
                });
            }

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
