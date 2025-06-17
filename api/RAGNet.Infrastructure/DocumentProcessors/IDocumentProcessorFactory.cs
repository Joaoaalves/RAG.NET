namespace RAGNET.Infrastructure.DocumentProcessors
{
    public interface IDocumentProcessorFactory
    {
        IDocumentProcessingService CreateDocumentProcessor(string fileExtension);
    }
}