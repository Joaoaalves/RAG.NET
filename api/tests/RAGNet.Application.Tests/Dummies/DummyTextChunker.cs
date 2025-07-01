using RAGNET.Application.Chunkers.Services;

namespace tests.RAGNet.Application.Tests.Dummies
{
    public class DummyTextChunker : ITextChunkerService
    {
        public Task<List<string>> ChunkText(string text)
        {
            var result = text
                .Split(['.'], StringSplitOptions.RemoveEmptyEntries)
                .Select(x => x.Trim())
                .Where(x => !string.IsNullOrWhiteSpace(x))
                .ToList();

            return Task.FromResult(result);
        }

        public decimal GetCostMultiplier() => 1;
    }
}