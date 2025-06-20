namespace RAGNET.Application.Providers
{
    public interface IQueryResultAggregatorService
    {
        List<VectorQueryResult> AggregateResults(List<VectorQueryResult> results, double? minScore, int topK = 5);
    }
}