using RAGNET.Domain.QueryResultFilters;

namespace RAGNET.Application.QueryResultFilters
{
    public interface IQueryResultFilterFactory
    {
        IQueryResultFilterService CreateQueryResultFilter(QueryResultFilter filter);
    }
}