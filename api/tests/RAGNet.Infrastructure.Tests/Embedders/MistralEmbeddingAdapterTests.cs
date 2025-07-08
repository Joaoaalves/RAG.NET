using System.Net;
using Moq;
using Moq.Protected;

using RAGNET.Domain.SharedKernel.VectorStorages.SemanticVectors;

using RAGNET.Infrastructure.Embedders.Mistral;

namespace tests.RAGNet.Infrastructure.Tests.Embedders;

public class MistralEmbeddingAdapterTests
{
  private HttpClient CreateHttpClient(string responseContent)
  {
    var handlerMock = new Mock<HttpMessageHandler>(MockBehavior.Strict);

    handlerMock
        .Protected()
        .Setup<Task<HttpResponseMessage>>(
            "SendAsync",
            ItExpr.IsAny<HttpRequestMessage>(),
            ItExpr.IsAny<CancellationToken>())
        .ReturnsAsync(new HttpResponseMessage
        {
          StatusCode = HttpStatusCode.OK,
          Content = new StringContent(responseContent)
        });

    return new HttpClient(handlerMock.Object)
    {
      BaseAddress = new Uri("https://api.mistral.ai/")
    };
  }

  [Fact]
  public async Task GetEmbeddingAsync_ReturnsVector()
  {
    var json = """
        {
          "data": [
            {
              "embedding": [1.0, 2.0, 3.0]
            }
          ]
        }
        """;

    var client = CreateHttpClient(json);
    var adapter = new MistralEmbeddingAdapter("fake-key", httpClient: client, delayMs: 1);

    var result = await adapter.GetEmbeddingAsync("test");

    var expected = new SemanticVector([1f, 2f, 3f]);
    Assert.Equal(expected, result);
  }

  [Fact]
  public async Task GetMultipleEmbeddingAsync_ReturnsMultipleVectors()
  {
    var json = """
        {
          "data": [
            { "embedding": [1.0, 2.0, 3.0] },
            { "embedding": [4.0, 5.0, 6.0] }
          ]
        }
        """;

    var client = CreateHttpClient(json);
    var adapter = new MistralEmbeddingAdapter("fake-key", httpClient: client, delayMs: 1);

    var result = await adapter.GetMultipleEmbeddingAsync(["text1", "text2"]);

    var expected = new[]
    {
            new SemanticVector([1f, 2f, 3f]),
            new SemanticVector([4f, 5f, 6f])
        };

    Assert.Equal(2, result.Count);
    Assert.Equal(expected[0], result[0]);
    Assert.Equal(expected[1], result[1]);
  }
}
