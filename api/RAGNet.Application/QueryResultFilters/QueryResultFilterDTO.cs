using System.Text.Json.Serialization;
using RAGNET.Application.QueryResultFilters;
using RAGNET.Domain.QueryResultFilters;

namespace RAGNET.Application.QueryResultFilters
{
    public class QueryResultFilterDTO
    {
        public Guid Id { get; set; }
        [JsonConverter(typeof(QueryResultFilterStrategyConverter))]
        public QueryResultFilterStrategy Strategy { get; set; }
        public int MaxItems { get; set; }
        public bool IsEnabled { get; set; }
    }
}