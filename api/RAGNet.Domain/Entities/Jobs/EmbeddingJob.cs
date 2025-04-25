namespace RAGNET.Domain.Entities.Jobs
{
    public class EmbeddingJob : Job
    {
        public string FileName { get; set; } = String.Empty;
        public byte[] FileContent { get; set; } = [];
    }
}