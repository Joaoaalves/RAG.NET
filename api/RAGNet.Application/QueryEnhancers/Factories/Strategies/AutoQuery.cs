using System.Text.Json;
using RAGNET.Application.Infrastructure.Providers.Conversation;
using RAGNET.Application.Queries;
using RAGNET.Application.Queries.Services;

namespace RAGNET.Application.QueryEnhancers.Factories.Strategies
{
    public class AutoQueryStrategy(string autoQueryPrompt, int maxQueries, string guidance, IConversationProviderService completionService) : IQueryEnhancerService
    {
        private readonly string _autoQueryPrompt = autoQueryPrompt;
        private readonly int _maxQueries = maxQueries;
        private readonly string _guidance = guidance;
        private readonly IConversationProviderService _completionService = completionService;
        public async Task<List<string>> GenerateQueries(string text)
        {
            List<string> queries = [];

            var promptWithGuidanceAndMaxQueries = _autoQueryPrompt.Replace("{GUIDANCE}", _guidance).Replace("{MAX_QUERIES}", _maxQueries.ToString());

            JsonDocument result = await _completionService.GetCompletionStructuredAsync(
                promptWithGuidanceAndMaxQueries,
                text,
                QuerySchema(),
                "AutoQuery"
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