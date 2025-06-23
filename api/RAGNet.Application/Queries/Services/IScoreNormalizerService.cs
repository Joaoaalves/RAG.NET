using RAGNET.Application.Infrastructure.Providers;

namespace RAGNET.Application.Queries.Services
{
    public interface IScoreNormalizerService
    {
        List<VectorQueryResult> MaybeNormalizeScores(
            List<VectorQueryResult> results,
            bool normalize,
            double? minNormalizedScore);
    }
}