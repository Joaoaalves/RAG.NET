using System.Text.Json;
using System.Text.Json.Serialization;
using RAGNET.Domain.Enums;

namespace RAGNET.Application.Converters
{
    public class FilterStrategyConverter : JsonConverter<FilterStrategy>
    {
        public override FilterStrategy Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options)
        {
            var value = reader.GetString();
            return value switch
            {
                "Relevant Segment Extraction" => FilterStrategy.RELEVANT_SEGMENT_EXTRACTION,
                _ => throw new ArgumentOutOfRangeException("Invalid Filter Strategy.")
            };
        }

        public override void Write(Utf8JsonWriter writer, FilterStrategy value, JsonSerializerOptions options)
        {
            var stringValue = value.ToString();
            writer.WriteStringValue(stringValue);
        }
    }
}