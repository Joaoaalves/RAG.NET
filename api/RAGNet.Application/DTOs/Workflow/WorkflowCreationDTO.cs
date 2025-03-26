using RAGNET.Application.DTOs.Chunker;
using RAGNET.Application.DTOs.Embedder;
using RAGNET.Domain.Enums;

namespace RAGNET.Application.DTOs.Workflow
{
    public class WorkflowCreationDTO
    {
        public string Name { get; set; } = String.Empty;
        public ChunkerStrategy Strategy { get; set; }
        public ChunkerSettingsDTO Settings { get; set; } = null!;
        public EmbeddingProviderConfigDTO EmbeddingProvider { get; set; } = null!;
    }
}