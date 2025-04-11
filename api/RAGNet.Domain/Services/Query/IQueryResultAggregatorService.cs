namespace RAGNET.Domain.Services.Query
{
    public interface IQueryResultAggregatorService
    {
        List<VectorQueryResult> AggregateResults(List<VectorQueryResult> results, double? minScore, int topK = 5);
    }
}