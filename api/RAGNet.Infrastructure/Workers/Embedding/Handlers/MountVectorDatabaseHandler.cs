using RAGNET.Application.Infrastructure.Providers.VectorDatabases;
using RAGNET.Application.Workflows.Commands.EnqueueEmbeddingJob.Jobs;

using RAGNET.Infrastructure.Jobs;

namespace RAGNET.Infrastructure.Workers.Embedding.Handlers
{
    public class MountVectorDatabaseHandler(
        IVectorDatabaseFactory vectorDatabaseFactory
    ) : BaseJobProcessingHandler
    {
        private readonly IVectorDatabaseFactory _vectorDatabaseFactory = vectorDatabaseFactory;
        public override async Task HandleAsync(EmbeddingJob job, CancellationToken ct)
        {
            var config = job.Context.Workflow.VectorStorageConfig;

            var vectorDbService = await _vectorDatabaseFactory.CreateVectorDatabaseServiceAsync(
                config.VectorStorageId,
                job.UserId
            );

            job.Context.VectorDatabaseService = vectorDbService;

            await base.HandleAsync(job, ct);
        }
    }
}