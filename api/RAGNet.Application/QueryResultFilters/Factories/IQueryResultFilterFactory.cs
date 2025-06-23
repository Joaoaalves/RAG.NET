using RAGNET.Application.QueryResultFilters.Services;
using RAGNET.Domain.QueryResultFilters;

namespace RAGNET.Application.QueryResultFilters.Factories
{
    public interface IQueryResultFilterFactory
    {
        IQueryResultFilterService CreateQueryResultFilter(QueryResultFilter filter);
    }
}