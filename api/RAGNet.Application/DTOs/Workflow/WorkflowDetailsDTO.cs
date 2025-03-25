using System.Text.Json.Serialization;
using RAGNET.Application.Converters;
using RAGNET.Application.DTOs.Chunker;
using RAGNET.Domain.Enums;

namespace RAGNET.Application.DTOs.Workflow
{
    public class WorkflowDetailsDTO
    {
        public string Name { get; set; } = String.Empty;
        [JsonConverter(typeof(ChunkerStrategyConverter))]
        public ChunkerStrategy Strategy { get; set; }
        public ChunkerSettingsDTO Settings { get; set; } = null!;
        public string ApiKey { get; set; } = string.Empty;
    }
}