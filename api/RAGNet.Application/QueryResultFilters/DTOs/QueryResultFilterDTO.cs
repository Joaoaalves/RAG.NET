using System.Text.Json.Serialization;
using RAGNET.Application.QueryResultFilters.DTOs.Converters;
using RAGNET.Domain.QueryResultFilters;

namespace RAGNET.Application.QueryResultFilters.DTOs
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