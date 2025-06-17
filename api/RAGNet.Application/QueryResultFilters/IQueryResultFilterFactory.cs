using RAGNET.Domain.Filters;

namespace RAGNET.Application.QueryResultFilters
{
    public interface IQueryResultFilterFactory
    {
        IQueryResultFilterService CreateContentFilter(Filter filter);
    }
}