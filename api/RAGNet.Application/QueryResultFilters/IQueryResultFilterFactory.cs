using RAGNET.Domain.Filters;

namespace RAGNET.Application.UserQueriesResultFilters
{
    public interface IQueryResultFilterFactory
    {
        IQueryResultFilterService CreateContentFilter(Filter filter);
    }
}