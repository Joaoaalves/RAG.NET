using RAGNET.Domain.Contexts;

namespace RAGNET.Domain.Entities.Jobs
{
    public class EmbeddingJob : Job<EmbeddingJobContext>
    {
        public string FileName { get; set; } = String.Empty;
        public byte[] FileContent { get; set; } = [];
    }
}