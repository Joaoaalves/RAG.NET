using System.Text.Json;
using System.Text.Json.Serialization;
using RAGNET.Domain.Enums;

namespace RAGNET.Application.Converters
{
    public class EmbeddingServiceConverter : JsonConverter<EmbeddingProviderEnum>
    {
        public override EmbeddingProviderEnum Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options)
        {
            var value = reader.GetString();
            return value switch
            {
                "OpenAI" => EmbeddingProviderEnum.OPENAI,
                _ => throw new ArgumentOutOfRangeException()
            };
        }

        public override void Write(Utf8JsonWriter writer, EmbeddingProviderEnum value, JsonSerializerOptions options)
        {
            var stringValue = value.ToString();
            writer.WriteStringValue(stringValue);
        }
    }
}