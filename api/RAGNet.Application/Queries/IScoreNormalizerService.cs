using RAGNET.Application.Providers;

namespace RAGNET.Application.Queries
{
    public interface IScoreNormalizerService
    {
        List<VectorQueryResult> MaybeNormalizeScores(
            List<VectorQueryResult> results,
            bool normalize,
            double? minNormalizedScore);
    }
}