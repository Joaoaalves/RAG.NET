using RAGNET.Domain.Entities;
using RAGNET.Domain.Services;

namespace RAGNET.Application.Services
{
    public class QueryResultAggregatorService : IQueryResultAggregatorService
    {
        public VectorQueryResult?[] AggregateResults(List<VectorQueryResult> results, int topK = 5)
        {
            var aggregatedResults = new Dictionary<string, VectorQueryResult>();

            foreach (var result in results)
            {
                if (aggregatedResults.TryGetValue(result.DocumentId, out VectorQueryResult? value))
                {
                    value.Score += result.Score;
                }
                else
                {
                    // Clone the result (or create a new instance) to avoid modifying the original object.
                    aggregatedResults[result.DocumentId] = new VectorQueryResult
                    {
                        DocumentId = result.DocumentId,
                        Score = result.Score,
                        // Copy metadata if needed.
                        Metadata = new Dictionary<string, string>(result.Metadata)
                    };
                }
            }

            // Order aggregated results by descending score and take the top K.
            var topResults = aggregatedResults.Values
                                              .OrderByDescending(r => r.Score)
                                              .Take(topK)
                                              .ToList();

            // Create an array of exactly K positions.
            VectorQueryResult?[] orderedResults = new VectorQueryResult?[topK];

            // Fill the positions.
            for (int i = 0; i < topK; i++)
            {
                orderedResults[i] = i < topResults.Count ? topResults[i] : null;
            }

            return orderedResults;
        }
    }
}