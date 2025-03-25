using System.Text.Json;
using System.Text.Json.Serialization;
using RAGNET.Domain.Enums;

namespace RAGNET.Application.Converters
{
    public class EmbeddingServiceConverter : JsonConverter<EmbeddingProviderEnum>
    {
        public override EmbeddingProviderEnum Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options)
        {
            if (reader.TokenType == JsonTokenType.String)
            {
                var enumString = reader.GetString();
                if (Enum.TryParse<EmbeddingProviderEnum>(enumString, true, out var value))
                {
                    return value;
                }
            }
            else if (reader.TokenType == JsonTokenType.Number)
            {
                if (reader.TryGetInt32(out int intValue))
                {
                    if (Enum.IsDefined(typeof(EmbeddingProviderEnum), intValue))
                    {
                        return (EmbeddingProviderEnum)intValue;
                    }
                }
            }
            throw new JsonException($"Unable to convert value to {nameof(EmbeddingProviderEnum)}.");
        }

        public override void Write(Utf8JsonWriter writer, EmbeddingProviderEnum value, JsonSerializerOptions options)
        {
            writer.WriteStringValue(value.ToString());
        }
    }
}