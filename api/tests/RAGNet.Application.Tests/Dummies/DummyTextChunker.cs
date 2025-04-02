using RAGNET.Domain.Services;

namespace tests.RAGNet.Application.Tests.Dummies
{
    public class DummyTextChunker : ITextChunkerService
    {
        public Task<IEnumerable<string>> ChunkText(string text)
        {
            var result = text
                .Split(['.'], StringSplitOptions.RemoveEmptyEntries)
                .Select(x => x.Trim())
                .Where(x => !string.IsNullOrWhiteSpace(x))
                .ToList();

            return Task.FromResult(result.AsEnumerable());
        }
    }
}