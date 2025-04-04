using System.Text.Json;
using System.Text.Json.Serialization;
using RAGNET.Domain.Enums;

namespace RAGNET.Application.Converters
{
    public class QueryEnhancerStrategyConverter : JsonConverter<QueryEnhancerStrategy>
    {
        public override QueryEnhancerStrategy Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options)
        {
            var value = reader.GetString();
            return value switch
            {
                "Auto Query" => QueryEnhancerStrategy.AUTO_QUERY,
                "Hypothetical Document Embedding" => QueryEnhancerStrategy.HYPOTHETICAL_DOCUMENT_EMBEDDING,
                _ => throw new ArgumentOutOfRangeException("Invalid Query Enhancer Strategy.")
            };
        }

        public override void Write(Utf8JsonWriter writer, QueryEnhancerStrategy value, JsonSerializerOptions options)
        {
            var stringValue = value.ToString();
            writer.WriteStringValue(stringValue);
        }
    }
}