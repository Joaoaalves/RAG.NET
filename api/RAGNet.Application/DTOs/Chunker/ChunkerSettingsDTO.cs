using System.Text.Json.Serialization;
using RAGNET.Application.Converters;
using RAGNET.Domain.Enums;

namespace RAGNET.Application.DTOs.Chunker
{
    public class ChunkerSettingsDTO
    {
        [JsonConverter(typeof(EmbeddingServiceConverter))]
        public EmbeddingProviderEnum EmbeddingService { get; set; }
        public double Threshold { get; set; }
        public string EvaluationPrompt { get; set; } = String.Empty;
        public int MaxChunkSize { get; set; }
    }
}