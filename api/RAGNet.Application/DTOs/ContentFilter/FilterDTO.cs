using System.Text.Json.Serialization;
using RAGNET.Application.QueryResultFilters;
using RAGNET.Domain.Filters;

namespace RAGNET.Application.DTOs.ContentFilter
{
    public class FilterDTO
    {
        public Guid Id { get; set; }
        [JsonConverter(typeof(QueryResultFilterStrategyConverter))]
        public FilterStrategyEnum Strategy { get; set; }
        public int MaxItems { get; set; }
        public bool IsEnabled { get; set; }
    }
}