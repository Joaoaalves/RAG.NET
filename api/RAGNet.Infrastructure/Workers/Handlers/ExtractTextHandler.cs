using RAGNet.Domain.Services;
using RAGNET.Domain.Entities;
using RAGNET.Domain.Entities.Jobs;
using RAGNET.Domain.Factories;
using RAGNET.Domain.Services.Queue;

namespace RAGNET.Infrastructure.Workers.Handlers
{
    public class ExtractTextHandler(IDocumentProcessorFactory documentProcessorFactory, IJobNotificationService realTimeNotifier) : BaseJobProcessingHandler
    {
        private readonly IDocumentProcessorFactory _documentProcessorFactory = documentProcessorFactory;
        public readonly IJobNotificationService _realTimeNotifier = realTimeNotifier;
        private readonly Process _currentProcess = new()
        {
            Title = "Extracting Document Pages",
            Progress = 100
        };

        public override async Task HandleAsync(EmbeddingJob job, CancellationToken ct)
        {

            await using var ms = new MemoryStream(job.FileContent);

            var ext = Path.GetExtension(job.FileName).ToLowerInvariant();
            var processor = _documentProcessorFactory.CreateDocumentProcessor(ext);

            var extract = await processor.ExtractTextAsync(ms);

            var document = await processor.CreateDocumentWithPagesAsync(
                                    Path.GetFileNameWithoutExtension(job.FileName),
                                    job.Context.Workflow.Id,
                                    extract.Pages
                                 );

            job.Context.ExtractResult = extract;
            job.Context.Document = document;

            await _realTimeNotifier.NotifyProgress(job.JobId, job.UserId, document, _currentProcess, ct);
            await base.HandleAsync(job, ct);
        }
    }
}