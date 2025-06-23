using System.Text.Json;
using System.Text.Json.Serialization;
using RAGNET.Domain.QueryResultFilters;

namespace RAGNET.Application.QueryResultFilters.DTOs.Converters
{
    public class QueryResultFilterStrategyConverter : JsonConverter<QueryResultFilterStrategy>
    {
        public override QueryResultFilterStrategy Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options)
        {
            var value = reader.GetString();
            return value switch
            {
                "Relevant Segment Extraction" => QueryResultFilterStrategy.RELEVANT_SEGMENT_EXTRACTION,
                _ => throw new ArgumentOutOfRangeException("Invalid QueryResultFilter Strategy.")
            };
        }

        public override void Write(Utf8JsonWriter writer, QueryResultFilterStrategy value, JsonSerializerOptions options)
        {
            var stringValue = value.ToString();
            writer.WriteStringValue(stringValue);
        }
    }
}