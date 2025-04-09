using RAGNET.Domain.Services;

namespace RAGNET.Application.Services
{
    public class QueryResultAggregatorService : IQueryResultAggregatorService
    {
        public List<VectorQueryResult> AggregateResults(List<VectorQueryResult> results, int topK = 5, double minNormalizedScore = 0.6)
        {
            var aggregatedResults = AggregateVectorResults(results);

            var topResults = GetTopResults(aggregatedResults, topK);

            NormalizeScores(topResults);

            return FillOrderedResults(topResults, topK, minNormalizedScore);
        }

        // Aggregates results by summing scores for entries with the same VectorId.
        private static Dictionary<string, VectorQueryResult> AggregateVectorResults(List<VectorQueryResult> results)
        {
            var aggregated = new Dictionary<string, VectorQueryResult>();

            foreach (var result in results)
            {
                if (aggregated.TryGetValue(result.VectorId, out VectorQueryResult? existingResult))
                {
                    // Sum the scores if the result already exists and continue to the next iteration.
                    existingResult.Score += result.Score;
                    continue;
                }
                // Add a clone of the result to avoid modifying the original object.
                aggregated[result.VectorId] = new VectorQueryResult
                {
                    VectorId = result.VectorId,
                    Score = result.Score,
                    Metadata = new Dictionary<string, string>(result.Metadata)
                };
            }

            return aggregated;
        }

        // Orders the aggregated results descending by score and takes the top K results.
        private static List<VectorQueryResult> GetTopResults(Dictionary<string, VectorQueryResult> aggregatedResults, int topK)
        {
            return aggregatedResults.Values
                                    .OrderByDescending(r => r.Score)
                                    .Take(topK)
                                    .ToList();
        }

        // Normalizes the scores of the given results, using the highest score as the divisor.
        private static void NormalizeScores(List<VectorQueryResult> results)
        {
            if (results.Count == 0) return;

            double maxScore = results.First().Score;
            foreach (var r in results)
            {
                r.Score /= maxScore;
            }
        }

        // Returns a list with a maximum of topK elements.
        private static List<VectorQueryResult> FillOrderedResults(List<VectorQueryResult> topResults, int topK, double minNormalizedScore)
        {
            var orderedResults = new List<VectorQueryResult>();
            for (int i = 0; i < topK; i++)
            {
                if (i < topResults.Count && topResults[i].Score >= minNormalizedScore)
                {
                    orderedResults.Add(topResults[i]);
                }
            }

            return orderedResults;
        }
    }
}
