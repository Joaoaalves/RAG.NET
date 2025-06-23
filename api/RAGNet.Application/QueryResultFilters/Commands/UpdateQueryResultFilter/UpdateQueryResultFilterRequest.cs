namespace RAGNET.Application.QueryResultFilters.Commands.UpdateQueryResultFilter
{
    public class QueryResultFilterUpdateRequest
    {
        public int MaxItems { get; set; }
        public bool? IsEnabled { get; set; } = true;

    }
}