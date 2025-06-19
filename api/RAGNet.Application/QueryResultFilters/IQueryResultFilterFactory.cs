using RAGNET.Domain.QueryResultFilters;

namespace RAGNET.Application.UserQueriesResultFilters
{
    public interface IQueryResultFilterFactory
    {
        IQueryResultFilterService CreateQueryResultFilter(QueryResultFilter filter);
    }
}