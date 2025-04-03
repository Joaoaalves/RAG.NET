using System.Text.Json;
using RAGNET.Domain.Services;

namespace RAGNET.Infrastructure.Adapters.QueryEnhancer
{
    public class HYDEAdapter(string hydePrompt, int maxQueries, IChatCompletionService completionService) : IQueryEnhancerService
    {
        private readonly string _hydePrompt = hydePrompt;
        private readonly int _maxQueries = maxQueries;
        private readonly IChatCompletionService _completionService = completionService;
        public async Task<List<string>> GenerateQueries(string text)
        {
            List<string> queries = [];
            var promptWithMaxQueries = _hydePrompt.Replace("{_maxQueries}", _maxQueries.ToString());

            JsonDocument result = await _completionService.GetCompletionStructuredAsync(
                promptWithMaxQueries,
                text,
                QuerySchema(),
                "HyDE"
            );

            if (result.RootElement.TryGetProperty("queries", out JsonElement queriesElement) && queriesElement.ValueKind == JsonValueKind.Array)
            {
                foreach (JsonElement query in queriesElement.EnumerateArray())
                {
                    queries.Add(query.GetString() ?? "");
                }
            }

            return [.. queries.Take(_maxQueries)];
        }

        private JsonDocument QuerySchema()
        {
            string querySchemaString = @"
            {
                ""type"" : ""object"",
                ""properties"" : {
                    ""queries"" : {
                        ""type"" : ""array"",
                        ""items"" : {""type"" : ""string""}
                    }
                },
                ""required"" : [""queries""],
                ""additionalProperties"" : false
            }
            ";

            return JsonDocument.Parse(querySchemaString);
        }
    }
}