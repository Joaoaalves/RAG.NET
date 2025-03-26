using RAGNET.Domain.Services;

namespace RAGNET.Infrastructure.Adapters.Chunking
{
    public class PropositionChunkerAdapter(int chunkSize, int overlap) : ITextChunkerService
    {
        private readonly int _chunkSize = chunkSize;
        private readonly int _overlap = overlap;

        public IEnumerable<string> ChunkText(string text)
        {
            var tokens = text.Split(new[] { ' ', '\n', '\r' }, StringSplitOptions.RemoveEmptyEntries);
            var chunks = new List<string>();

            for (int i = 0; i < tokens.Length; i += (_chunkSize - _overlap))
            {
                var chunkTokens = tokens.Skip(i).Take(_chunkSize);
                chunks.Add(string.Join(" ", chunkTokens));
                if (i + _chunkSize >= tokens.Length)
                    break;
            }
            return chunks;
        }
    }
}