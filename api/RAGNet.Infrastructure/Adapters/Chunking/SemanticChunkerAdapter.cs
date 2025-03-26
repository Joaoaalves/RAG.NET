// Infrastructure/Adapters/Chunking/SemanticChunker.cs
using System.Text;
using RAGNET.Domain.Services;

namespace RAGNET.Infrastructure.Adapters.Chunking
{
    public class SemanticChunkerAdapter(int chunkSize, int overlap) : ITextChunkerService
    {
        private readonly int _chunkSize = chunkSize;
        private readonly int _overlap = overlap;

        public IEnumerable<string> ChunkText(string text)
        {
            var sentences = text.Split(new[] { ". ", "! ", "? " }, StringSplitOptions.RemoveEmptyEntries);
            var chunks = new List<string>();
            var chunk = new StringBuilder();

            foreach (var sentence in sentences)
            {
                if (chunk.Length + sentence.Length > _chunkSize)
                {
                    chunks.Add(chunk.ToString().Trim());

                    chunk.Clear();
                }
                chunk.Append(sentence).Append(". ");
            }
            if (chunk.Length > 0)
                chunks.Add(chunk.ToString().Trim());
            return chunks;
        }
    }
}
