namespace RAGNET.Domain.Services.Query
{
    public interface IQueryResultAggregatorService
    {
        List<VectorQueryResult> AggregateResults(List<VectorQueryResult> results, int topK = 5);
    }
}