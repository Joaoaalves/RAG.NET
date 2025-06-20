namespace RAGNET.Application.QueryResultFilters.UpdateQueryResultFilter
{
    public class QueryResultFilterUpdateRequest
    {
        public int MaxItems { get; set; }
        public bool? IsEnabled { get; set; } = true;

    }
}