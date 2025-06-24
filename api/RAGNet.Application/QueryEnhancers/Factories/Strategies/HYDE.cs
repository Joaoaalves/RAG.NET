using System.Text.Json;
using RAGNET.Application.Infrastructure.Providers.Conversation;
using RAGNET.Application.Queries.Services;


namespace RAGNET.Application.QueryEnhancers.Factories.Strategies
{
    public class HYDEStrategy(string hydePrompt, int maxQueries, IConversationProviderService completionService) : IQueryEnhancerService
    {
        private readonly string _hydePrompt = hydePrompt;
        private readonly int _maxQueries = maxQueries;
        private readonly IConversationProviderService _completionService = completionService;
        public async Task<List<string>> GenerateQueries(string text)
        {
            List<string> queries = [];
            var promptWithMaxQueries = _hydePrompt.Replace("{MAX_QUERIES}", _maxQueries.ToString());

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

        public decimal GetCostMultiplier() => 1.01m;

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