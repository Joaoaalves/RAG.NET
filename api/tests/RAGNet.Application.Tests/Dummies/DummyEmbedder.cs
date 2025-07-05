using RAGNET.Application.Infrastructure.Providers.Embedding;
using RAGNET.Domain.Documents.Pages.Chunks;

namespace tests.RAGNet.Application.Tests.Dummies
{
    public class DummyEmbedder : IEmbeddingService
    {
        private static readonly SemanticVector result = new([1.0f, 2.0f, 3.0f]);

        public Task<SemanticVector> GetEmbeddingAsync(string chunk)
        {
            return Task.FromResult(result);
        }

        public Task<List<SemanticVector>> GetMultipleEmbeddingAsync(List<string> texts)
        {
            var embeddings = texts.Select(_ => result).ToList();
            return Task.FromResult(embeddings);
        }
    }
}