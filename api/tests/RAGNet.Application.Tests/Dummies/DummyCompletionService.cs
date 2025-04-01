using System.Text.Json;
using RAGNET.Domain.Services;

namespace tests.RAGNet.Application.Tests.Dummies
{
    public class DummyCompletionService : IChatCompletionService
    {

        public Task<string> GetCompletionAsync(string systemPrompt, string message)
        {
            throw new NotImplementedException();
        }

        public Task<JsonDocument> GetCompletionStructuredAsync(string systemPrompt, string message, JsonDocument jsonSchema, string? formatName)
        {
            throw new NotImplementedException();
        }
    }
}