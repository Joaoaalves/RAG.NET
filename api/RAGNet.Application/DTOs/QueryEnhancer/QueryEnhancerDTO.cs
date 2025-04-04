using System.Text.Json.Serialization;
using RAGNET.Application.Converters;
using RAGNET.Domain.Enums;

namespace RAGNET.Application.DTOs.QueryEnhancer
{
    public class QueryEnhancerDTO
    {
        public Guid Id { get; set; }

        [JsonConverter(typeof(QueryEnhancerStrategyConverter))]
        public QueryEnhancerStrategy Type { get; set; }
        public int MaxQueries { get; set; }
        public string? Guidance { get; set; }
    }
}