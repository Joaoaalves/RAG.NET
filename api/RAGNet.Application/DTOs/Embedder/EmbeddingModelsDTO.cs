using RAGNET.Domain.Entities;

namespace RAGNET.Application.DTOs.Embedder
{
    public class EmbeddingModelsDTO
    {
        public List<EmbeddingModel> Anthropic { get; set; } = [];
        public List<EmbeddingModel> OpenAI { get; set; } = [];
    }
}