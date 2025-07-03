using System.Text.Json;
using Moq;
using OpenAI.Chat;
using RAGNET.Infrastructure.ChatCompletions.OpenAI;

namespace tests.RAGNet.Infrastructure.Tests.ChatCompletions;

public class OpenAIChatAdapterTests
{
    private readonly Mock<IOpenAIChatClientWrapper> _mockClient;
    private readonly int _delayMs = 1;

    public OpenAIChatAdapterTests()
    {
        _mockClient = new Mock<IOpenAIChatClientWrapper>(MockBehavior.Strict);
    }

    private OpenAIChatAdapter CreateAdapter() => new(_mockClient.Object, _delayMs);

    [Fact]
    public async Task GetCompletionAsync_ReturnsExpectedText()
    {
        // Arrange
        var expectedText = "Hello, world!";
        _mockClient
            .Setup(c => c.CompleteChatAsync(It.IsAny<ChatMessage[]>(), null, default))
            .ReturnsAsync(expectedText);

        var adapter = CreateAdapter();

        // Act
        var result = await adapter.GetCompletionAsync("sys", "user");

        // Assert
        Assert.Equal(expectedText, result);
    }

    [Fact]
    public async Task GetCompletionAsync_RetriesOnFailure_ThenReturns()
    {
        // Arrange
        var attempt = 0;
        _mockClient
            .Setup(c => c.CompleteChatAsync(It.IsAny<ChatMessage[]>(), null, default))
            .ReturnsAsync(() =>
            {
                attempt++;
                if (attempt < 3)
                    throw new HttpRequestException("internal");
                return "Recovered!";
            });

        var adapter = CreateAdapter();

        // Act
        var result = await adapter.GetCompletionAsync("sys", "user");

        // Assert
        Assert.Equal("Recovered!", result);
        Assert.Equal(3, attempt);
    }

    [Fact]
    public async Task GetCompletionStructuredAsync_ReturnsJsonDocument()
    {
        // Arrange
        var json = "{\"foo\": \"bar\"}";
        var formatName = "mySchema";
        var schema = JsonDocument.Parse("{\"type\":\"object\"}");

        _mockClient
            .Setup(c => c.CompleteChatAsync(It.IsAny<ChatMessage[]>(), It.IsAny<ChatCompletionOptions>(), default))
            .ReturnsAsync(json);

        var adapter = CreateAdapter();

        // Act
        var result = await adapter.GetCompletionStructuredAsync("sys", "user", schema, formatName);

        // Assert
        Assert.Equal("bar", result.RootElement.GetProperty("foo").GetString());
    }

    [Fact]
    public async Task GetCompletionStructuredAsync_RetriesOnFailure_ThenReturns()
    {
        // Arrange
        var json = "{\"key\": 123}";
        var formatName = "schema";
        var schema = JsonDocument.Parse("{}");
        int attempts = 0;

        _mockClient
            .Setup(c => c.CompleteChatAsync(It.IsAny<ChatMessage[]>(), It.IsAny<ChatCompletionOptions>(), default))
            .ReturnsAsync(() =>
            {
                attempts++;
                if (attempts < 2)
                    throw new HttpRequestException("timeout");
                return json;
            });

        var adapter = CreateAdapter();

        // Act
        var result = await adapter.GetCompletionStructuredAsync("sys", "msg", schema, formatName);

        // Assert
        Assert.Equal(123, result.RootElement.GetProperty("key").GetInt32());
        Assert.Equal(2, attempts);
    }

    [Fact]
    public async Task GetCompletionStructuredAsync_Throws_OnInvalidJson()
    {
        // Arrange
        var invalidJson = "this is not json";
        var schema = JsonDocument.Parse("{}");

        _mockClient
            .Setup(c => c.CompleteChatAsync(It.IsAny<ChatMessage[]>(), It.IsAny<ChatCompletionOptions>(), default))
            .ReturnsAsync(invalidJson);

        var adapter = CreateAdapter();

        // Act & Assert
        await Assert.ThrowsAnyAsync<JsonException>(() =>
            adapter.GetCompletionStructuredAsync("sys", "msg", schema, "format"));
    }
}
