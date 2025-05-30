using System.Text.Json.Serialization;

namespace RAGNET.Application.DTOs
{
    public class EmbeddingProgressDTO
    {

        [JsonPropertyName("processedChunks")]
        public int ProcessedChunks { get; set; }
        [JsonPropertyName("totalChunks")]
        public int TotalChunks { get; set; }
    }
}