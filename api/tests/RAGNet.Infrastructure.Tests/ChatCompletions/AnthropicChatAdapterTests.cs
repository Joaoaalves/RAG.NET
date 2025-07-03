using System.Text.Json;
using Anthropic.SDK.Messaging;
using Moq;
using RAGNET.Infrastructure.ChatCompletions.Anthropic;
using RAGNET.Infrastructure.Exceptions.Adapters;

namespace tests.RAGNet.Infrastructure.Tests.ChatCompletions
{
    public class AnthropicChatAdapterTests
    {
        private static MessageResponse BuildResponse(string? text)
        {
            return new MessageResponse
            {
                Content =
                [
                    new TextContent { Text = text }
                ]
            };
        }
        [Fact]
        public async Task GetCompletionAsync_ReturnsText_WhenSuccessful()
        {
            var mock = new Mock<IAnthropicClientWrapper>();
            mock.Setup(c => c.CompleteChatAsync(It.IsAny<MessageParameters>()))
                .ReturnsAsync(BuildResponse("Hello world"));

            var adapter = new AnthropicChatAdapter(mock.Object);

            var result = await adapter.GetCompletionAsync("system", "user");

            Assert.Equal("Hello world", result);
        }

        [Fact]
        public async Task GetCompletionAsync_ThrowsException_WhenTextIsNull()
        {
            var mock = new Mock<IAnthropicClientWrapper>();
            mock.Setup(c => c.CompleteChatAsync(It.IsAny<MessageParameters>()))
                .ReturnsAsync(BuildResponse(null));

            var adapter = new AnthropicChatAdapter(mock.Object);

            var ex = await Assert.ThrowsAsync<AnthropicChatException>(() =>
                adapter.GetCompletionAsync("sys", "msg"));

            Assert.Contains("Error occurred while calling Anthropic API.", ex.Message);
        }

        [Fact]
        public async Task GetCompletionAsync_ThrowsException_WhenClientFails()
        {
            var mock = new Mock<IAnthropicClientWrapper>();
            mock.Setup(c => c.CompleteChatAsync(It.IsAny<MessageParameters>()))
                .ThrowsAsync(new InvalidOperationException("fail"));

            var adapter = new AnthropicChatAdapter(mock.Object);

            var ex = await Assert.ThrowsAsync<AnthropicChatException>(() =>
                adapter.GetCompletionAsync("sys", "msg"));

            Assert.Contains("Anthropic API", ex.Message);
        }

        [Fact]
        public async Task GetCompletionStructuredAsync_ReturnsJson_WhenSuccessful()
        {
            var json = """{"name": "test"}""";
            var mock = new Mock<IAnthropicClientWrapper>();
            mock.Setup(c => c.CompleteChatAsync(It.IsAny<MessageParameters>()))
                .ReturnsAsync(BuildResponse(json));

            var adapter = new AnthropicChatAdapter(mock.Object);

            var schema = JsonDocument.Parse("""{"type":"object"}""");

            var result = await adapter.GetCompletionStructuredAsync("sys", "msg", schema, null);

            Assert.Equal("test", result.RootElement.GetProperty("name").GetString());
        }

        [Fact]
        public async Task GetCompletionStructuredAsync_ThrowsException_OnInvalidJson()
        {
            var mock = new Mock<IAnthropicClientWrapper>();
            mock.Setup(c => c.CompleteChatAsync(It.IsAny<MessageParameters>()))
                .ReturnsAsync(BuildResponse("{ invalid ")); // malformed JSON

            var adapter = new AnthropicChatAdapter(mock.Object);

            var schema = JsonDocument.Parse("""{"type":"object"}""");

            var ex = await Assert.ThrowsAsync<AnthropicChatException>(() =>
                adapter.GetCompletionStructuredAsync("sys", "msg", schema, null));

            Assert.Contains("structured JSON", ex.Message);
        }

        [Fact]
        public async Task GetCompletionAsync_RetriesOnFailure_ThenReturnsText()
        {
            var mock = new Mock<IAnthropicClientWrapper>();

            int callCount = 0;
            mock.Setup(c => c.CompleteChatAsync(It.IsAny<MessageParameters>()))
                .ReturnsAsync(() =>
                {
                    callCount++;
                    if (callCount < 3)
                        throw new HttpRequestException("429");
                    return BuildResponse("Retried success");
                });

            var adapter = new AnthropicChatAdapter(mock.Object, delayMs: 1);

            var result = await adapter.GetCompletionAsync("sys", "msg");

            Assert.Equal("Retried success", result);
            Assert.Equal(3, callCount); // 2 fails + 1 success
        }

        [Fact]
        public async Task GetCompletionAsync_ThrowsException_AfterMaxRetries()
        {
            var mock = new Mock<IAnthropicClientWrapper>();

            mock.Setup(c => c.CompleteChatAsync(It.IsAny<MessageParameters>()))
                .ThrowsAsync(new HttpRequestException("429"));

            var adapter = new AnthropicChatAdapter(mock.Object, delayMs: 1);

            var ex = await Assert.ThrowsAsync<AnthropicChatException>(() =>
                adapter.GetCompletionAsync("sys", "msg"));

            Assert.Contains("Anthropic API", ex.Message);
            mock.Verify(m => m.CompleteChatAsync(It.IsAny<MessageParameters>()), Times.Exactly(6)); // tentativa inicial + 3 retries
        }
    }
}
