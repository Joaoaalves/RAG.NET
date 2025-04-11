namespace RAGNET.Domain.Services.Query
{
    public interface IScoreNormalizerService
    {
        List<VectorQueryResult> MaybeNormalizeScores(
            List<VectorQueryResult> results,
            bool normalize,
            double? minNormalizedScore);
    }
}