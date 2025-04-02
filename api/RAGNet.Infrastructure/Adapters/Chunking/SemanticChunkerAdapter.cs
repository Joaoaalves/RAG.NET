using System.Text;
using System.Text.Json;
using System.Text.RegularExpressions;
using RAGNET.Domain.Services;

namespace RAGNET.Infrastructure.Adapters.Chunking
{
    public class SemanticChunkerAdapter(float threshold, IChatCompletionService completionService) : ITextChunkerService
    {
        private readonly float _threshold = threshold;
        private readonly IChatCompletionService _completionService = completionService;

        public async Task<IEnumerable<string>> ChunkText(string text)
        {
            var sentences = Regex.Split(text, @"(?<=[\.!\?])\s+");

            List<string> chunks = [];

            if (sentences.Length == 0)
                return chunks;

            StringBuilder currentChunk = new(sentences[0].Trim());

            for (int i = 1; i < sentences.Length; i++)
            {
                string candidateSentence = sentences[i].Trim();

                string systemMessage = BuildSystemMessage(currentChunk);
                string message = $"\"\nCandidate Sentence: \"{candidateSentence}\"\n";

                JsonDocument evaluationResult = await _completionService.GetCompletionStructuredAsync(
                    systemMessage,
                    message,
                    GetSchema(),
                    "SemanticEvaluation"
                );

                double similarity = 0;
                if (evaluationResult.RootElement.TryGetProperty("similarity", out JsonElement similarityElement))
                {
                    similarity = similarityElement.GetDouble();
                }

                if (similarity >= _threshold)
                {
                    currentChunk.Append(" " + candidateSentence);
                }
                else
                {
                    chunks.Add(currentChunk.ToString());
                    currentChunk.Clear();
                    currentChunk.Append(candidateSentence);
                }
            }

            if (currentChunk.Length > 0)
            {
                chunks.Add(currentChunk.ToString());
            }

            return chunks;
        }

        private JsonDocument GetSchema()
        {
            string schemaString = @"
                {
                    ""type"": ""object"",
                    ""properties"": {
                        ""similarity"": { ""type"": ""number"" }
                    },
                    ""required"": [""similarity""]
                }";
            return JsonDocument.Parse(schemaString);
        }

        private string BuildSystemMessage(StringBuilder chunk)
        {
            return $"Current Chunk: \"{chunk}" +
                                 "Please evaluate the semantic similarity between the current chunk and the candidate sentence in percentage (0 <= eval <= 1).";

        }
    }
}
