using System.Text.Json.Serialization;
using RAGNET.Application.Converters;
using RAGNET.Domain.Enums;

namespace RAGNET.Application.DTOs.ContentFilter
{
    public class FilterDTO
    {
        public Guid Id { get; set; }
        [JsonConverter(typeof(FilterStrategyConverter))]
        public FilterStrategy Strategy { get; set; }
        public int MaxItems { get; set; }
        public bool IsEnabled { get; set; }
    }
}