using System.Text.Json;
using System.Text.Json.Serialization;
using RAGNET.Domain.QueryResultFilters;

namespace RAGNET.Application.UserQueriesResultFilters
{
    public class QueryResultFilterStrategyConverter : JsonConverter<QueryResultFilterStrategyEnum>
    {
        public override QueryResultFilterStrategyEnum Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options)
        {
            var value = reader.GetString();
            return value switch
            {
                "Relevant Segment Extraction" => QueryResultFilterStrategyEnum.RELEVANT_SEGMENT_EXTRACTION,
                _ => throw new ArgumentOutOfRangeException("Invalid QueryResultFilter Strategy.")
            };
        }

        public override void Write(Utf8JsonWriter writer, QueryResultFilterStrategyEnum value, JsonSerializerOptions options)
        {
            var stringValue = value.ToString();
            writer.WriteStringValue(stringValue);
        }
    }
}