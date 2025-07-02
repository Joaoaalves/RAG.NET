using RAGNET.Application.Workflows.Commands.EnqueueEmbeddingJob.Jobs;
using RAGNET.Domain.Documents;
using RAGNET.Domain.Documents.Pages;
using RAGNET.Infrastructure.DocumentProcessors;
using RAGNET.Infrastructure.Jobs;
using RAGNET.Infrastructure.Jobs.Queue;

namespace RAGNET.Infrastructure.Workers.Handlers
{
    public class ExtractTextHandler(IDocumentProcessorFactory documentProcessorFactory, IJobNotificationService realTimeNotifier) : BaseJobProcessingHandler
    {
        private readonly IDocumentProcessorFactory _documentProcessorFactory = documentProcessorFactory;
        public readonly IJobNotificationService _realTimeNotifier = realTimeNotifier;
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

            job.Context.ExtractResult = extract;
            job.Context.Document = document;
            await _realTimeNotifier.NotifyProgress(job.JobId, job.UserId, document, _currentProcess, ct);
            await base.HandleAsync(job, ct);
        }
    }
}