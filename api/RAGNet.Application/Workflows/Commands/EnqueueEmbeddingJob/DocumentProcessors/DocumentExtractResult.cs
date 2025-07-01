namespace RAGNET.Application.Workflows.Commands.EnqueueEmbeddingJob.DocumentProcessors
{
    public class DocumentExtractResult
    {
        public string DocumentTitle { get; set; } = String.Empty;
        public List<string> Pages { get; set; } = [];
    }

}