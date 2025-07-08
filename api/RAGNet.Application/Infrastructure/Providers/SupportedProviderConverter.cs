using System.Text.Json;
using System.Text.Json.Serialization;
using RAGNET.Domain.SharedKernel.Providers;

namespace RAGNET.Application.Infrastructure.Providers
{
    public class SupportedProviderConverter : JsonConverter<SupportedProvider>
    {
        public override SupportedProvider Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options)
        {
            var value = reader.GetString();
            return value switch
            {
                "OpenAI" => SupportedProvider.OPENAI,
                "Anthropic" => SupportedProvider.ANTHROPIC,
                "Voyage" => SupportedProvider.VOYAGE,
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