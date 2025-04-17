using System.Text.Json;
using System.Text.Json.Serialization;
using RAGNET.Domain.Enums;

namespace RAGNET.Application.Converters
{
    public class SupportedProviderConverter : JsonConverter<SupportedProvider>
    {
        public override SupportedProvider Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options)
        {
            var value = reader.GetString();
            return value switch
            {
                "OpenAI" => SupportedProvider.OpenAI,
                "Anthropic" => SupportedProvider.Anthropic,
                "Voyage" => SupportedProvider.Voyage,
                "QDrant" => SupportedProvider.QDrant,
                _ => throw new ArgumentOutOfRangeException()
            };
        }

        public override void Write(Utf8JsonWriter writer, SupportedProvider value, JsonSerializerOptions options)
        {
            var stringValue = value.ToString();
            writer.WriteStringValue(stringValue);
        }
    }
}