using RAGNET.Domain.Documents;
using RAGNET.Domain.SeedWork;

namespace RAGNET.Infrastructure.DocumentProcessors
{
    public class DocumentProcessorFactory(IDocumentRepository documentRepository, IUnitOfWork unitOfWork) : IDocumentProcessorFactory
    {
        private readonly IDocumentRepository _documentRepository = documentRepository;
        private readonly IUnitOfWork _unitOfWork = unitOfWork;
        public IDocumentProcessingService CreateDocumentProcessor(string fileExtension)
        {
            return fileExtension.ToLower() switch
            {
                ".pdf" => new PDFProcessingAdapter(_documentRepository, _unitOfWork),
                ".epub" => new EpubProcessingAdapter(_documentRepository, _unitOfWork),
                _ => throw new NotSupportedException($"File extension '{fileExtension}' is not supported.")
            };
        }
    }
}