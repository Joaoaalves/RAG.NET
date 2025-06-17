using RAGNET.Infrastructure.Jobs.Contexts;

namespace RAGNET.Infrastructure.Jobs
{
    public class EmbeddingJob : Job<EmbeddingJobContext>
    {
        public string FileName { get; set; } = String.Empty;
        public byte[] FileContent { get; set; } = [];
    }
}