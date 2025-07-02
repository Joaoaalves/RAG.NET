using RAGNET.Application.Workflows.Commands.EnqueueEmbeddingJob.Contexts;

namespace RAGNET.Application.Workflows.Commands.EnqueueEmbeddingJob.Jobs
{
    public class EmbeddingJob : Job<EmbeddingJobContext>
    {
        public string FileName { get; set; } = String.Empty;
        public byte[] FileContent { get; set; } = [];
    }
}