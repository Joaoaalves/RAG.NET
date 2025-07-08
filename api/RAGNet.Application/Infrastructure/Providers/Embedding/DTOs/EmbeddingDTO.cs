using RAGNET.Domain.SharedKernel.VectorStorages.SemanticVectors;

namespace RAGNET.Application.Infrastructure.Providers.Embedding.DTOs
{
    public class EmbeddingDTO
    {
        public string VectorId { get; set; } = string.Empty;
        public SemanticVector Vector { get; set; } = null!;
        public Dictionary<string, string> Metadata { get; set; } = [];
        public string ChunkText { get; set; } = "";
    }
}