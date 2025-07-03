using System.Text.Json;
using Moq;
using RAGNET.Infrastructure.ChatCompletions.OpenAI;
using RAGNET.Infrastructure.ChatCompletions.xAI;

namespace tests.RAGNet.Infrastructure.Tests.ChatCompletions
{
    public class XAIChatAdapterTests
    {
        private readonly Mock<IOpenAIChatClientWrapper> _mockWrapper;
        private readonly int _delayMs = 1;

        public XAIChatAdapterTests()
        {
            _mockWrapper = new Mock<IOpenAIChatClientWrapper>(MockBehavior.Strict);
        }

        private XAIChatAdapter CreateAdapter() => new(_mockWrapper.Object, _delayMs);

        [Fact]
        public async Task GetCompletionAsync_ReturnsExpectedText()
        {
            // Arrange
            var expectedText = "Expected response";

            _mockWrapper
                .Setup(c => c.CompleteChatAsync(It.IsAny<OpenAI.Chat.ChatMessage[]>(), null, default))
                .ReturnsAsync(expectedText);

            var adapter = CreateAdapter();

            // Act
            var result = await adapter.GetCompletionAsync("system", "message");

            // Assert
            Assert.Equal(expectedText, result);
        }

        [Fact]
        public async Task GetCompletionStructuredAsync_ReturnsParsedJson()
        {
            // Arrange
            var responseJson = "{\"value\":\"ok\"}";
            var expected = "ok";

            _mockWrapper
                .Setup(c => c.CompleteChatAsync(It.IsAny<OpenAI.Chat.ChatMessage[]>(), It.IsAny<OpenAI.Chat.ChatCompletionOptions>(), default))
                .ReturnsAsync(responseJson);

            var adapter = CreateAdapter();
            var schema = JsonDocument.Parse("{\"type\":\"object\"}");

            // Act
            var result = await adapter.GetCompletionStructuredAsync("sys", "msg", schema, "xaiFormat");

            // Assert
            Assert.NotNull(result);
            Assert.Equal(expected, result.RootElement.GetProperty("value").GetString());
        }

        [Fact]
        public async Task GetCompletionStructuredAsync_ThrowsOnInvalidJson()
        {
            // Arrange
            var invalidJson = "{invalid json";
            _mockWrapper
                .Setup(c => c.CompleteChatAsync(It.IsAny<OpenAI.Chat.ChatMessage[]>(), It.IsAny<OpenAI.Chat.ChatCompletionOptions>(), default))
                .ReturnsAsync(invalidJson);

            var adapter = CreateAdapter();
            var schema = JsonDocument.Parse("{}");

            // Act & Assert
            var ex = await Assert.ThrowsAnyAsync<JsonException>(() =>
                adapter.GetCompletionStructuredAsync("sys", "msg", schema, "f"));

            Assert.Contains("Failed to parse", ex.Message, StringComparison.OrdinalIgnoreCase);
        }

        [Fact]
        public async Task GetCompletionAsync_RetriesAndSucceeds()
        {
            // Arrange
            int attempts = 0;

            _mockWrapper
                .Setup(c => c.CompleteChatAsync(It.IsAny<OpenAI.Chat.ChatMessage[]>(), null, default))
                .ReturnsAsync(() =>
                {
                    attempts++;
                    if (attempts < 3) throw new HttpRequestException("timeout");
                    return "Retried response";
                });

            var adapter = CreateAdapter();

            // Act
            var result = await adapter.GetCompletionAsync("sys", "msg");

            // Assert
            Assert.Equal("Retried response", result);
            Assert.True(attempts >= 3);
        }

        [Fact]
        public async Task GetCompletionStructuredAsync_RetriesAndSucceeds()
        {
            // Arrange
            int attempts = 0;
            var successJson = "{\"success\":true}";
            var schema = JsonDocument.Parse("{\"type\":\"object\"}");

            _mockWrapper
                .Setup(c => c.CompleteChatAsync(It.IsAny<OpenAI.Chat.ChatMessage[]>(), It.IsAny<OpenAI.Chat.ChatCompletionOptions>(), default))
                .ReturnsAsync(() =>
                {
                    attempts++;
                    if (attempts < 2) throw new HttpRequestException("timeout");
                    return successJson;
                });

            var adapter = CreateAdapter();

            // Act
            var result = await adapter.GetCompletionStructuredAsync("sys", "msg", schema, "format");

            // Assert
            Assert.True(result.RootElement.GetProperty("success").GetBoolean());
            Assert.True(attempts >= 2);
        }
    }
}
