using System.Text.Json;
using System.Text.Json.Serialization;
using RAGNET.Domain.Enums;

namespace RAGNET.Application.Converters
{
    public class ChunkerStrategyConverter : JsonConverter<ChunkerStrategy>
    {
        public override ChunkerStrategy Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options)
        {
            var value = reader.GetString();
            return value switch
            {
                "PROPOSITION" => ChunkerStrategy.PROPOSITION,
                "SEMANTIC" => ChunkerStrategy.SEMANTIC,
                _ => throw new ArgumentOutOfRangeException()
            };
        }

        public override void Write(Utf8JsonWriter writer, ChunkerStrategy value, JsonSerializerOptions options)
        {
            var stringValue = value.ToString();
            writer.WriteStringValue(stringValue);
        }
    }
}