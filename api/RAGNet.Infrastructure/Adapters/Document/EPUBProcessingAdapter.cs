using Microsoft.AspNetCore.Http;
using RAGNET.Domain.Entities;
using RAGNET.Domain.Repositories;
using RAGNET.Domain.Services;
using VersOne.Epub;

namespace RAGNET.Infrastructure.Adapters.Document
{
    public class EpubProcessingAdapter(
        IDocumentRepository documentRepository,
        IPageRepository pageRepository) : IDocumentProcessingService
    {
        private readonly IDocumentRepository _documentRepository = documentRepository;
        private readonly IPageRepository _pageRepository = pageRepository;

        private const int WordsPerPage = 400;

        public async Task<DocumentExtractResult> ExtractTextAsync(IFormFile file)
        {
            var result = new DocumentExtractResult();

            using var stream = file.OpenReadStream();
            EpubBook epubBook = await EpubReader.ReadBookAsync(stream);

            foreach (var item in epubBook.ReadingOrder)
            {
                if (item != null && !string.IsNullOrWhiteSpace(item.Content))
                {
                    List<string> pages = PaginateText(item.Content, WordsPerPage);
                    result.Pages.AddRange(pages);
                }
            }

            return result;
        }

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
        private List<string> PaginateText(string text, int wordsPerPage)
        {
            var words = text.Split([' ', '\n', '\r'], StringSplitOptions.RemoveEmptyEntries);
            var pages = new List<string>();

            for (int i = 0; i < words.Length; i += wordsPerPage)
            {
                var pageWords = words.Skip(i).Take(wordsPerPage);
                pages.Add(string.Join(" ", pageWords));
            }

            return pages;
        }
    }
}

