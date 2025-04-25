using RAGNet.Domain.Services;
using RAGNET.Domain.Entities.Jobs;
using RAGNET.Domain.Factories;

namespace RAGNET.Infrastructure.Workers.Handlers
{
    public class ExtractTextHandler(IDocumentProcessorFactory documentProcessorFactory) : BaseJobProcessingHandler
    {
        private readonly IDocumentProcessorFactory _documentProcessorFactory = documentProcessorFactory;
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

            await base.HandleAsync(job, ct);
        }
    }
}