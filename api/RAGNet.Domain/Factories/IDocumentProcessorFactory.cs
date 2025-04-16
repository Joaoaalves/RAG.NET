using RAGNET.Domain.Services;

namespace RAGNET.Domain.Factories
{
    public interface IDocumentProcessorFactory
    {
        IDocumentProcessingService CreateDocumentProcessor(string fileExtension);
    }
}