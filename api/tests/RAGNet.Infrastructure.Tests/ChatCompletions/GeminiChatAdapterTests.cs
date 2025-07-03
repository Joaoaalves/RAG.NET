using System.Net;
using System.Text.Json;
using RAGNET.Infrastructure.ChatCompletions;
using RAGNET.Infrastructure.ChatCompletions.Gemini;
using RAGNET.Infrastructure.Exceptions.Adapters;
using RichardSzalay.MockHttp;

namespace tests.RAGNet.Infrastructure.Tests.ChatCompletions;

public class GeminiChatAdapterTests
{
    private static GeminiChatAdapter CreateAdapter(string responseJson, HttpStatusCode statusCode = HttpStatusCode.OK)
    {
        var mockHttp = new MockHttpMessageHandler();

        mockHttp.When("*").Respond(statusCode, "application/json", responseJson);

        var client = mockHttp.ToHttpClient();
        client.BaseAddress = new Uri("https://generativelanguage.googleapis.com/");

        return new GeminiChatAdapter("fake-key", "gemini-2.0", client, 1);
    }

    [Fact]
    public async Task GetCompletionAsync_ReturnsExpectedText()
    {
        // Arrange
        var response = """
        {
            "candidates": [
                {
                    "content": {
                        "parts": [
                            { "text": "Hello, world!" }
                        ]
                    }
                }
            ]
        }
        """;

        var adapter = CreateAdapter(response);

        // Act
        var result = await adapter.GetCompletionAsync("System", "Message");

        // Assert
        Assert.Equal("Hello, world!", result);
    }

    [Fact]
    public async Task GetCompletionAsync_ThrowsGeminiException_OnHttpError()
    {
        // Arrange
        var adapter = CreateAdapter("rate limit", HttpStatusCode.TooManyRequests);

        // Act & Assert
        var ex = await Assert.ThrowsAsync<GeminiChatException>(() =>
            adapter.GetCompletionAsync("System", "Message"));

        Assert.Contains("Gemini API error", ex.Message);
    }

    [Fact]
    public async Task GetCompletionAsync_ThrowsGeminiException_OnInvalidJson()
    {
        // Arrange
        var adapter = CreateAdapter("not a json");

        // Act & Assert
        var ex = await Assert.ThrowsAsync<GeminiChatException>(() =>
            adapter.GetCompletionAsync("System", "Message"));

        Assert.Contains("Failed to parse JSON", ex.Message);
    }

    [Fact]
    public async Task GetCompletionStructuredAsync_ReturnsStructuredJson()
    {
        // Arrange
        var wrappedJson = """
        {
            "candidates": [
                {
                    "content": {
                        "parts": [
                            {
                                "text": "{ \"foo\": \"bar\" }"
                            }
                        ]
                    }
                }
            ]
        }
        """;

        var adapter = CreateAdapter(wrappedJson);

        var schema = JsonDocument.Parse("""
        {
            "type": "object",
            "properties": {
                "foo": { "type": "string" }
            }
        }
        """);

        // Act
        var result = await adapter.GetCompletionStructuredAsync("Sys", "Msg", schema, "whatever");

        // Assert
        Assert.Equal("bar", result.RootElement.GetProperty("foo").GetString());
    }

    [Fact]
    public async Task GetCompletionStructuredAsync_ThrowsGeminiException_OnInvalidWrappedJson()
    {
        // Arrange
        var adapter = CreateAdapter("not valid json");

        var schema = JsonDocument.Parse("{}");

        // Act & Assert
        var ex = await Assert.ThrowsAsync<GeminiChatException>(() =>
            adapter.GetCompletionStructuredAsync("sys", "msg", schema, "json"));

        Assert.Contains("Failed to parse structured JSON", ex.Message);
    }

    [Fact]
    public async Task GetCompletionStructuredAsync_ThrowsGeminiException_OnHttpError()
    {
        // Arrange
        var adapter = CreateAdapter("some error", HttpStatusCode.Forbidden);
        var schema = JsonDocument.Parse("{}");

        // Act & Assert
        var ex = await Assert.ThrowsAsync<GeminiChatException>(() =>
            adapter.GetCompletionStructuredAsync("sys", "msg", schema, null));

        Assert.Contains("Gemini API error", ex.Message);
    }
}
