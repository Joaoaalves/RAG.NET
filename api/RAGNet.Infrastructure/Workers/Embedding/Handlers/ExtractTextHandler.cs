using RAGNET.Domain.Documents;
using RAGNET.Domain.Documents.Pages;

using RAGNET.Application.Workflows.Commands.EnqueueEmbeddingJob.Jobs;

using RAGNET.Infrastructure.DocumentProcessors;
using RAGNET.Infrastructure.Jobs;
using RAGNET.Infrastructure.Jobs.Queue;

namespace RAGNET.Infrastructure.Workers.Embedding.Handlers
{
    public class ExtractTextHandler(
        IDocumentProcessorFactory documentProcessorFactory,
        IJobNotificationService realTimeNotifier
    ) : NotifierJobProcessingHandler(realTimeNotifier)
    {
        private readonly IDocumentProcessorFactory _documentProcessorFactory = documentProcessorFactory;
        private readonly ProcessDTO _currentProcess = new()
        {
            Title = "Extracting Document Pages",
            Progress = 100
        };

        public override async Task HandleAsync(EmbeddingJob job, CancellationToken ct)
        {
            await using var ms = new MemoryStream(job.FileContent);

            var ext = Path.GetExtension(job.FileName).ToLowerInvariant();
            var processor = _documentProcessorFactory.CreateDocumentProcessor(ext);

            var fileTitle = new Text(Path.GetFileNameWithoutExtension(job.FileName));

            var extract = await processor.ExtractTextAsync(ms);

            var document = Document.Create(
                fileTitle,
                job.Context.Workflow.Id
            );

            document = await processor.CreateDocumentWithPagesAsync(
                                    document,
                                    extract.Pages
                                 );

            job.Context.Document = document;
            await NotifyProgress(job, _currentProcess, ct);
            await base.HandleAsync(job, ct);
        }
    }
}