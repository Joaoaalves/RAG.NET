using System.Text.Json;

namespace RAGNET.Application.Providers.Conversation
{
    public interface IChatCompletionService
    {
        Task<string> GetCompletionAsync(string systemPrompt, string message);
        Task<JsonDocument> GetCompletionStructuredAsync(string systemPrompt, string message, JsonDocument jsonSchema, string? formatName);
    }
}