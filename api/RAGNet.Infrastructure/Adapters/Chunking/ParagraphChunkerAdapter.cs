using RAGNET.Domain.Services;

namespace RAGNET.Infrastructure.Adapters.Chunking
{
    public class ParagraphChunkerAdapter(int maxChunkSize) : ITextChunkerService
    {
        private readonly int _maxChunkSize = maxChunkSize;

        public Task<IEnumerable<string>> ChunkText(string text)
        {
            var paragraphs = text.Split(['\n'], StringSplitOptions.RemoveEmptyEntries);
            var chunks = new List<string>();

            foreach (var paragraph in paragraphs)
            {
                if (paragraph.Length <= _maxChunkSize)
                {
                    chunks.Add(paragraph);
                }
                else
                {
                    chunks.AddRange(SplitIntoChunks(paragraph, _maxChunkSize));
                }
            }

            return Task.FromResult<IEnumerable<string>>(chunks);
        }

        private IEnumerable<string> SplitIntoChunks(string paragraph, int chunkSize)
        {
            for (int i = 0; i < paragraph.Length; i += chunkSize)
            {
                yield return paragraph.Substring(i, Math.Min(chunkSize, paragraph.Length - i));
            }
        }
    }
}