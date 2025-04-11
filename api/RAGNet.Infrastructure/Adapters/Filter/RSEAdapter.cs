using System.Text.Json;
using RAGNET.Domain.Entities;
using RAGNET.Domain.Services;
using RAGNET.Domain.Services.Filter;

namespace RAGNET.Infrastructure.Adapters.Filter
{
    public class RSEFilter(string prompt, int maximumItems) : IContentFilterService
    {
        public async Task<List<string>> FilterContent(List<ContentItem> contentItems, string query, IChatCompletionService completionProvider)
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