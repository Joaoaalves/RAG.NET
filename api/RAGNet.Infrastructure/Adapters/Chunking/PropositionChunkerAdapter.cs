using System.Collections.Concurrent;
using System.Text.Json;
using RAGNET.Domain.Services;

namespace RAGNET.Infrastructure.Adapters.Chunking
{
    public class PropositionChunkerAdapter : ITextChunkerService
    {
        private readonly float _threshold;
        private readonly string _chunkPrompt;
        private readonly string _evaluationPrompt;
        private readonly IChatCompletionService _completionService;

        public PropositionChunkerAdapter(float threshold, string chunkPrompt, string evaluationPrompt, IChatCompletionService completionService)
        {
            _threshold = threshold;
            _chunkPrompt = chunkPrompt;
            _evaluationPrompt = evaluationPrompt;
            _completionService = completionService;
        }

        public async Task<IEnumerable<string>> ChunkText(string text)
        {
            var paragraphs = text.Split(['\n'], StringSplitOptions.RemoveEmptyEntries);
            var chunksBag = new ConcurrentBag<string>(); // Thread-safe list

            Console.WriteLine($"Chunking text with {paragraphs.Length} paragraphs");

            await Parallel.ForEachAsync(paragraphs, async (paragraph, _) =>
            {
                Console.WriteLine($"Processing paragraph: {paragraph}");

                var chunks = await ChunkParagraph(paragraph);
                var evaluatedChunks = await EvaluateChunks(chunks);

                foreach (var chunk in evaluatedChunks)
                {
                    chunksBag.Add(chunk);
                }
            });

            return chunksBag;
        }

        private async Task<List<string>> ChunkParagraph(string paragraph)
        {
            JsonDocument result = await _completionService.GetCompletionStructuredAsync(
                _chunkPrompt,
                paragraph,
                ChunkerSchema(),
                "Chunking"
            );

            List<string> chunks = [];

            if (result.RootElement.TryGetProperty("chunks", out JsonElement chunksElement) &&
                chunksElement.ValueKind == JsonValueKind.Array)
            {
                foreach (JsonElement item in chunksElement.EnumerateArray())
                {
                    if (item.ValueKind == JsonValueKind.String)
                    {
                        chunks.Add(item.GetString() ?? "");
                    }
                }
            }

            return chunks;
        }

        private async Task<List<string>> EvaluateChunks(List<string> chunks)
        {
            string chunksSerialized = JsonSerializer.Serialize(chunks);

            var evaluationResult = await _completionService.GetCompletionStructuredAsync(
                _evaluationPrompt,
                chunksSerialized,
                EvaluatorSchema(),
                "Evaluation_Chunking"
            );

            List<string> filteredChunks = [];

            if (evaluationResult.RootElement.TryGetProperty("chunks", out JsonElement evaluatedChunksElement) &&
                evaluatedChunksElement.ValueKind == JsonValueKind.Array)
            {
                foreach (JsonElement evaluatedChunk in evaluatedChunksElement.EnumerateArray())
                {
                    double accuracy = evaluatedChunk.GetProperty("accuracy").GetDouble();
                    double clarity = evaluatedChunk.GetProperty("clarity").GetDouble();
                    double completeness = evaluatedChunk.GetProperty("completeness").GetDouble();
                    double conciseness = evaluatedChunk.GetProperty("conciseness").GetDouble();

                    double averageScore = (accuracy + clarity + completeness + conciseness) / 4.0;
                    double normalizedScore = averageScore / 10.0;

                    if (normalizedScore >= _threshold)
                    {
                        string text = evaluatedChunk.GetProperty("text").GetString() ?? "";
                        filteredChunks.Add(text);
                    }
                }
            }

            return filteredChunks;
        }

        private JsonDocument ChunkerSchema()
        {
            string chunkSchemaString = @"
            {
                ""type"": ""object"",
                ""properties"": {
                    ""chunks"": {
                        ""type"": ""array"",
                        ""items"": { ""type"": ""string"" }
                    }
                },
                ""required"": [""chunks""],
                ""additionalProperties"": false
            }";
            return JsonDocument.Parse(chunkSchemaString);
        }

        private JsonDocument EvaluatorSchema()
        {
            string evaluationSchemaString = @"
            {
                ""type"": ""object"",
                ""properties"": {
                    ""chunks"": {
                        ""type"": ""array"",
                        ""items"": {
                            ""type"": ""object"",
                            ""properties"": {
                                ""text"": { ""type"": ""string"" },
                                ""accuracy"": { ""type"": ""number"" },
                                ""clarity"": { ""type"": ""number"" },
                                ""completeness"": { ""type"": ""number"" },
                                ""conciseness"": { ""type"": ""number"" }
                            },
                            ""required"": [""text"", ""accuracy"", ""clarity"", ""completeness"", ""conciseness""],
                            ""additionalProperties"": false
                        }
                    }
                },
                ""required"": [""chunks""],
                ""additionalProperties"": false
            }";
            return JsonDocument.Parse(evaluationSchemaString);
        }
    }
}
