using RAGNET.Domain.Services;
using RAGNET.Domain.Services.Query;

namespace RAGNET.Application.Services
{
    public class ScoreNormalizerService : IScoreNormalizerService
    {
        public List<VectorQueryResult> MaybeNormalizeScores(
            List<VectorQueryResult> results,
            bool normalize,
            double? minNormalizedScore)
        {
            if (!normalize || results.Count == 0)
                return results;

            double maxScore = results.First().Score;

            return results
                .Select(r => new VectorQueryResult
                {
                    VectorId = r.VectorId,
                    Score = r.Score / maxScore,
                    Metadata = r.Metadata
                })
                .Where(r => minNormalizedScore == null || r.Score >= minNormalizedScore)
                .ToList();
        }
    }

}