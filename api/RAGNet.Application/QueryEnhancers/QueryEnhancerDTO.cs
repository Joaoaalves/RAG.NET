using System.Text.Json.Serialization;
using RAGNET.Application.UserQueriesEnhancers;

using RAGNET.Domain.QueryEnhancers;

namespace RAGNET.Application.QueryEnhancers
{
    public class QueryEnhancerDTO
    {
        public Guid Id { get; set; }
        [JsonConverter(typeof(QueryEnhancerStrategyConverter))]
        public QueryEnhancerStrategy Type { get; set; }
        public int MaxQueries { get; set; }
        public bool IsEnabled { get; set; } = true;
        public string? Guidance { get; set; }
    }
}