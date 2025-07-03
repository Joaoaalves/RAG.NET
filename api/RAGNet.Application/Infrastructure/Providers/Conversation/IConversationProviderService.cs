using System.Text.Json;

namespace RAGNET.Application.Infrastructure.Providers.Conversation
{
    public interface IConversationProviderService
    {
        Task<string> GetCompletionAsync(string systemPrompt, string message);
        Task<JsonDocument> GetCompletionStructuredAsync(string systemPrompt, string message, JsonDocument jsonSchema, string? formatName = null);
    }
}