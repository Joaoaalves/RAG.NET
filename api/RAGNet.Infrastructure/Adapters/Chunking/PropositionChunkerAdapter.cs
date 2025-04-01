using RAGNET.Domain.Services;

namespace RAGNET.Infrastructure.Adapters.Chunking
{
    public class PropositionChunkerAdapter : ITextChunkerService
    {
        private readonly int _chunkSize;
        private readonly int _overlap;
        private readonly string _chunkPrompt;
        private readonly string _evaluationPrompt;
        private readonly IChatCompletionService _completionService;

        public PropositionChunkerAdapter(int chunkSize, int overlap, string chunkPrompt, string evaluationPrompt, IChatCompletionService completionService)
        {
            _chunkSize = chunkSize;
            _overlap = overlap;
            _chunkPrompt = chunkPrompt;
            _evaluationPrompt = evaluationPrompt;
            _completionService = completionService;
        }

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
