using System.Text.Json;
using DeepSeek.Core.Models;
using Moq;
using RAGNET.Infrastructure.ChatCompletions.DeepSeek;

namespace tests.RAGNet.Infrastructure.Tests.ChatCompletions;

public class DeepSeekChatAdapterTests
{
    private readonly Mock<IDeepSeekClientWrapper> _mockClient;
    private readonly int _delayMs = 1;

    public DeepSeekChatAdapterTests()
    {
        _mockClient = new Mock<IDeepSeekClientWrapper>(MockBehavior.Strict);
    }

    private DeepSeekChatAdapter CreateAdapter() => new(_mockClient.Object, _delayMs);

    [Fact]
    public async Task GetCompletionAsync_ReturnsExpectedText()
    {
        // Arrange
        var expected = "This is a test response";
        var systemPrompt = "system";
        var userMessage = "user";

        _mockClient
            .Setup(x => x.CompleteChatAsync(It.IsAny<List<Message>>(), null, default))
            .ReturnsAsync(expected);

        var adapter = CreateAdapter();

        // Act
        var result = await adapter.GetCompletionAsync(systemPrompt, userMessage);

        // Assert
        Assert.Equal(expected, result);
    }

    [Fact]
    public async Task GetCompletionStructuredAsync_ReturnsJsonDocument_OnSuccess()
    {
        // Arrange
        var systemPrompt = "system";
        var userMessage = "user";
        var expectedJson = "{\"result\":\"ok\"}";

        var schema = JsonDocument.Parse("""
        {
            "type": "object",
            "properties": {
                "result": { "type": "string" }
            }
        }
        """);

        _mockClient
            .Setup(x => x.CompleteChatAsync(It.IsAny<List<Message>>(), It.Is<ResponseFormat>(rf => rf.Type == "json_object"), default))
            .ReturnsAsync(expectedJson);

        var adapter = CreateAdapter();

        // Act
        var result = await adapter.GetCompletionStructuredAsync(systemPrompt, userMessage, schema);

        // Assert
        Assert.NotNull(result);
        Assert.Equal("ok", result.RootElement.GetProperty("result").GetString());
    }

    [Fact]
    public async Task GetCompletionStructuredAsync_Throws_WhenJsonInvalid()
    {
        // Arrange
        var systemPrompt = "system";
        var userMessage = "user";
        var formatName = "format";
        var schema = JsonDocument.Parse("{}");

        _mockClient
            .Setup(x => x.CompleteChatAsync(It.IsAny<List<Message>>(), It.IsAny<ResponseFormat>(), default))
            .ReturnsAsync("not-json");

        var adapter = CreateAdapter();

        // Act & Assert
        await Assert.ThrowsAsync<InvalidOperationException>(() =>
            adapter.GetCompletionStructuredAsync(systemPrompt, userMessage, schema, formatName));
    }

    [Fact]
    public async Task GetCompletionAsync_RetriesOnFailure_ThenSucceeds()
    {
        // Arrange
        var systemPrompt = "system";
        var userMessage = "user";
        var expected = "eventual success";
        int attempts = 0;

        _mockClient
            .Setup(x => x.CompleteChatAsync(It.IsAny<List<Message>>(), null, default))
            .ReturnsAsync(() =>
            {
                attempts++;
                if (attempts < 3)
                    throw new HttpRequestException("timeout");
                return expected;
            });

        var adapter = CreateAdapter();

        // Act
        var result = await adapter.GetCompletionAsync(systemPrompt, userMessage);

        // Assert
        Assert.Equal(expected, result);
        Assert.True(attempts >= 3);
    }

    [Fact]
    public async Task GetCompletionStructuredAsync_RetriesOnFailure_ThenSucceeds()
    {
        // Arrange
        var systemPrompt = "system";
        var userMessage = "user";
        var formatName = "any";
        var schema = JsonDocument.Parse("{}");
        var expectedJson = "{}";

        int attempts = 0;

        _mockClient
            .Setup(x => x.CompleteChatAsync(It.IsAny<List<Message>>(), It.IsAny<ResponseFormat>(), default))
            .ReturnsAsync(() =>
            {
                attempts++;
                if (attempts < 2)
                    throw new HttpRequestException("timeout");
                return expectedJson;
            });

        var adapter = CreateAdapter();

        // Act
        var result = await adapter.GetCompletionStructuredAsync(systemPrompt, userMessage, schema, formatName);

        // Assert
        Assert.NotNull(result);
        Assert.True(attempts >= 2);
    }
}
