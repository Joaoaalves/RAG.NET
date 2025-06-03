using System.Text.Json;
using System.Text.Json.Serialization;
using RAGNET.Domain.SharedKernel.Providers;

namespace RAGNET.Application.Converters
{
    public class ConversationServiceConverter : JsonConverter<ConversationProviderEnum>
    {
        public override ConversationProviderEnum Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options)
        {
            if (reader.TokenType == JsonTokenType.String)
            {
                var enumString = reader.GetString();
                if (Enum.TryParse<ConversationProviderEnum>(enumString, true, out var value))
                {
                    return value;
                }
            }
            else if (reader.TokenType == JsonTokenType.Number)
            {
                if (reader.TryGetInt32(out int intValue))
                {
                    if (Enum.IsDefined(typeof(ConversationProviderEnum), intValue))
                    {
                        return (ConversationProviderEnum)intValue;
                    }
                }
            }
            throw new JsonException($"Unable to convert value to {nameof(ConversationProviderEnum)}.");
        }

        public override void Write(Utf8JsonWriter writer, ConversationProviderEnum value, JsonSerializerOptions options)
        {
            writer.WriteStringValue(value.ToString());
        }
    }
}