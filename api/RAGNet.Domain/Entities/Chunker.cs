using RAGNET.Domain.Enums;

namespace RAGNET.Domain.Entities
{
    public class Chunker
    {
        private string Id { get; set; } = String.Empty;
        private ChunkerTypeEnum Type { get; set; }
        private float Threshold
        { get; set; } = 0.9F;
        private int MaxChunkSize { get; set; } = 600;
        private string PromptEvaluator { get; set; } = String.Empty;
        private string PromptChunker { get; set; } = String.Empty;

        /* Relations
            EmbeddingProvider
        */
    }
}