using RAGNET.Domain.Services;
using System.Linq;
namespace tests.RAGNet.Application.Tests.Dummies
{
    public class DummyEmbedder : IEmbeddingService
    {
        private static readonly float[] result = [1.0f, 2.0f, 3.0f];

        public Task<float[]> GetEmbeddingAsync(string chunk)
        {
            return Task.FromResult(result);
        }

        public Task<List<float[]>> GetMultipleEmbeddingAsync(List<string> texts)
        {
            var embeddings = texts.Select(_ => result).ToList();
            return Task.FromResult(embeddings);
        }
    }
}