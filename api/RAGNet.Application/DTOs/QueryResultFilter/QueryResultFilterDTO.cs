using System.Text.Json.Serialization;
using RAGNET.Application.UserQueriesResultFilters;
using RAGNET.Domain.QueryResultFilters;

namespace RAGNET.Application.DTOs.QueryResultFilter
{
    public class QueryResultFilterDTO
    {
        public Guid Id { get; set; }
        [JsonConverter(typeof(QueryResultFilterStrategyConverter))]
        public QueryResultFilterStrategyEnum Strategy { get; set; }
        public int MaxItems { get; set; }
        public bool IsEnabled { get; set; }
    }
}