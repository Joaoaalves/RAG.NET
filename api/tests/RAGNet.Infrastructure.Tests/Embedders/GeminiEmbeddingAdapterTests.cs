using System.Net;
using RAGNET.Infrastructure.Embedders;
using RAGNET.Infrastructure.Exceptions.Adapters;
using RichardSzalay.MockHttp;

namespace tests.RAGNet.Infrastructure.Tests.Embedders
{
    public class GeminiEmbeddingAdapterTests
    {
        private static GeminiEmbeddingAdapter CreateAdapter(string responseBody, HttpStatusCode statusCode = HttpStatusCode.OK)
        {
            var mockHttp = new MockHttpMessageHandler();

            mockHttp.When("*").Respond(statusCode, "application/json", responseBody);

            var client = mockHttp.ToHttpClient();
            client.BaseAddress = new Uri("https://generativelanguage.googleapis.com/");

            return new GeminiEmbeddingAdapter("fake-api-key", "gemini-embedding-exp-03-07", client, 1);
        }

        [Fact]
        public async Task GetEmbeddingAsync_ReturnsFloatArray_OnSuccess()
        {
            // Arrange
            var responseJson = """
            {
                "embedding": {
                    "values": [0.1, 0.2, 0.3]
                }
            }
            """;

            var adapter = CreateAdapter(responseJson);

            // Act
            var result = await adapter.GetEmbeddingAsync("test");

            // Assert
            Assert.Equal([0.1f, 0.2f, 0.3f], result);
        }

        [Fact]
        public async Task GetEmbeddingAsync_ThrowsGeminiException_OnHttpError()
        {
            // Arrange
            var adapter = CreateAdapter("some error", HttpStatusCode.TooManyRequests);

            // Act & Assert
            var ex = await Assert.ThrowsAsync<HttpRequestException>(() =>
                adapter.GetEmbeddingAsync("test"));
        }

        [Fact]
        public async Task GetEmbeddingAsync_ThrowsGeminiException_OnInvalidJson()
        {
            // Arrange
            var adapter = CreateAdapter("not-json");

            // Act & Assert
            var ex = await Assert.ThrowsAsync<GeminiEmbeddingException>(() =>
                adapter.GetEmbeddingAsync("test"));

            Assert.Contains("Failed to parse JSON response from Gemini", ex.Message);
        }

        [Fact]
        public async Task GetMultipleEmbeddingAsync_ReturnsMultipleVectors()
        {
            // Arrange
            var responseJson = """
            {
                "embedding": {
                    "values": [0.5, 0.6]
                }
            }
            """;

            var adapter = CreateAdapter(responseJson);

            // Act
            var result = await adapter.GetMultipleEmbeddingAsync(["a", "b"]);

            // Assert
            Assert.Equal(2, result.Count);
            Assert.All(result, emb => Assert.Equal([0.5f, 0.6f], emb));
        }
    }
}