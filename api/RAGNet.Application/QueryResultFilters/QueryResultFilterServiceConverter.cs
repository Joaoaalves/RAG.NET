using System.Text.Json;
using System.Text.Json.Serialization;
using RAGNET.Domain.Filters;

namespace RAGNET.Application.UserQueriesResultFilters
{
    public class QueryResultFilterStrategyConverter : JsonConverter<FilterStrategyEnum>
    {
        public override FilterStrategyEnum Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options)
        {
            var value = reader.GetString();
            return value switch
            {
                "Relevant Segment Extraction" => FilterStrategyEnum.RELEVANT_SEGMENT_EXTRACTION,
                _ => throw new ArgumentOutOfRangeException("Invalid Filter Strategy.")
            };
        }

        public override void Write(Utf8JsonWriter writer, FilterStrategyEnum value, JsonSerializerOptions options)
        {
            var stringValue = value.ToString();
            writer.WriteStringValue(stringValue);
        }
    }
}