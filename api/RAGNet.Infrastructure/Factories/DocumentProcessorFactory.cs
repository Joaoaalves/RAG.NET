using RAGNET.Domain.Factories;
using RAGNET.Domain.Repositories;
using RAGNET.Domain.Services;
using RAGNET.Infrastructure.Adapters.Documents;

namespace RAGNET.Infrastructure.Factories
{
    public class DocumentProcessorFactory(IDocumentRepository documentRepository, IPageRepository pageRepository) : IDocumentProcessorFactory
    {
        private readonly IDocumentRepository _documentRepository = documentRepository;
        private readonly IPageRepository _pageRepository = pageRepository;
        public IDocumentProcessingService CreateDocumentProcessor(string fileExtension)
        {
            return fileExtension.ToLower() switch
            {
                ".pdf" => new PDFProcessingAdapter(_documentRepository, _pageRepository),
                ".epub" => new EpubProcessingAdapter(_documentRepository, _pageRepository),
                _ => throw new NotSupportedException($"File extension '{fileExtension}' is not supported.")
            };
        }
    }
}