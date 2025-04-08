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
                if (aggregatedResults.TryGetValue(result.VectorId, out VectorQueryResult? value))
                {
                    value.Score += result.Score;
                }
                else
                {
                    // Clone the result to avoid modifying the original object.
                    aggregatedResults[result.VectorId] = new VectorQueryResult
                    {
                        VectorId = result.VectorId,
                        Score = result.Score,
                        Metadata = new Dictionary<string, string>(result.Metadata)
                    };
                }
            }

            // Order aggregated results by descending score and take the top K.
            var topResults = aggregatedResults.Values
                                              .OrderByDescending(r => r.Score)
                                              .Take(topK)
                                              .ToList();

            // Normalize the score
            double maxScore = topResults.Count != 0 ? topResults.First().Score : 1.0;
            foreach (var r in topResults)
            {
                r.Score /= maxScore;
            }

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