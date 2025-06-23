using System.Text.Json;

using RAGNET.Application.Infrastructure.Providers.Conversation;
using RAGNET.Application.Queries.DTOs;
using RAGNET.Application.QueryResultFilters.Services;

namespace RAGNET.Application.QueryResultFilters.Factories.Strategies
{
    public class RSEFilterStrategy(string prompt, int maximumItems) : IQueryResultFilterService
    {
        public async Task<List<string>> FilterContent(List<ContentItemDTO> contentItems, string query, IConversationProviderService completionProvider)
        {
            prompt = prompt.Replace("{user_query}", query);

            string chunksString = "Chunks:\n";
            foreach (var item in contentItems)
            {
                chunksString += $"\n\n{item.Text}";
            }

            JsonDocument result = await completionProvider.GetCompletionStructuredAsync(prompt, chunksString, FilterSchema(), "RSE_Filter");

            List<string> chunks = [];

            if (result.RootElement.TryGetProperty("chunks", out JsonElement chunksElement) &&
                chunksElement.ValueKind == JsonValueKind.Array)
            {
                foreach (JsonElement item in chunksElement.EnumerateArray())
                {
                    if (item.ValueKind == JsonValueKind.String)
                    {
                        chunks.Add(item.GetString() ?? "");

                        if (chunks.Count >= maximumItems)
                            break;
                    }
                }
            }

            return chunks;
        }

        private JsonDocument FilterSchema()
        {
            string filterSchemaString = @"
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
            }
            ";
            return JsonDocument.Parse(filterSchemaString);
        }
    }
}