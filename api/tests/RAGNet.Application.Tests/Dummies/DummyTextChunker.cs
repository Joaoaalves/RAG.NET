using RAGNET.Domain.Services;

namespace tests.RAGNet.Application.Tests.Dummies
{
    public class DummyTextChunker : ITextChunkerService
    {
        public IEnumerable<string> ChunkText(string text)
        {
            return text
                .Split(['.'], StringSplitOptions.RemoveEmptyEntries)
                .Select(x => x.Trim())
                .Where(x => !string.IsNullOrWhiteSpace(x))
                .ToList();
        }
    }
}