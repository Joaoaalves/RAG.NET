using System.Net;
using System.Text.Json;
using RAGNET.Infrastructure.Embedders;
using RichardSzalay.MockHttp;
using Xunit;

namespace tests.RAGNet.Infrastructure.Tests.Embedders
{
    public class VoyageEmbeddingAdapterTests
    {
        private static VoyageEmbeddingAdapter CreateAdapter(string responseBody, HttpStatusCode statusCode = HttpStatusCode.OK)
        {
            var mockHttp = new MockHttpMessageHandler();

            mockHttp.When("https://api.voyageai.com/v1/embeddings")
                    .Respond(statusCode, "application/json", responseBody);

            var client = mockHttp.ToHttpClient();
            client.BaseAddress = new Uri("https://api.voyageai.com/v1/");

            return new VoyageEmbeddingAdapter("fake-api-key", "voyage-code-2", client, delayMs: 1);
        }

        [Fact]
        public async Task GetEmbeddingAsync_ReturnsFloatArray_OnSuccess()
        {
            // Arrange
            var responseJson = """
            {
                "data": [
                    {
                        "embedding": [0.1, 0.2, 0.3]
                    }
                ]
            }
            """;

            var adapter = CreateAdapter(responseJson);

            // Act
            var result = await adapter.GetEmbeddingAsync("hello");

            // Assert
            Assert.Equal(new[] { 0.1f, 0.2f, 0.3f }, result);
        }

        [Fact]
        public async Task GetEmbeddingAsync_Throws_OnHttpError()
        {
            // Arrange
            var adapter = CreateAdapter("error!", HttpStatusCode.TooManyRequests);

            // Act & Assert
            var ex = await Assert.ThrowsAsync<HttpRequestException>(() =>
                adapter.GetEmbeddingAsync("test"));

            Assert.Contains("429", ex.Message);
        }

        [Fact]
        public async Task GetEmbeddingAsync_Throws_OnInvalidJson()
        {
            // Arrange
            var adapter = CreateAdapter("invalid-json");

            // Act & Assert
            await Assert.ThrowsAnyAsync<JsonException>(() =>
                adapter.GetEmbeddingAsync("test"));
        }

        [Fact]
        public async Task GetMultipleEmbeddingAsync_ReturnsMultipleEmbeddings()
        {
            // Arrange
            var responseJson = """
            {
                "data": [
                    {
                        "embedding": [0.9, 0.8]
                    }
                ]
            }
            """;

            var adapter = CreateAdapter(responseJson);

            // Act
            var result = await adapter.GetMultipleEmbeddingAsync(new List<string> { "a", "b" });

            // Assert
            Assert.Equal(2, result.Count);
            Assert.All(result, emb => Assert.Equal(new[] { 0.9f, 0.8f }, emb));
        }
    }
}
