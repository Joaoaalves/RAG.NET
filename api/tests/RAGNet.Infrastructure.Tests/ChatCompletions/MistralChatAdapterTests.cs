using System.Net;
using System.Text.Json;
using Moq;
using Moq.Protected;
using RAGNET.Infrastructure.ChatCompletions.Mistral;

namespace tests.RAGNet.Infrastructure.Tests.ChatCompletions;

public class MistralChatAdapterTests
{
    private static HttpClient CreateHttpClient(string responseContent)
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

    private static HttpClient CreateHttpClientSequence(params HttpResponseMessage[] responses)
    {
        var handlerMock = new Mock<HttpMessageHandler>(MockBehavior.Strict);
        var sequence = handlerMock
            .Protected()
            .SetupSequence<Task<HttpResponseMessage>>(
                "SendAsync",
                ItExpr.IsAny<HttpRequestMessage>(),
                ItExpr.IsAny<CancellationToken>());

        foreach (var response in responses)
            sequence = sequence.ReturnsAsync(response);

        return new HttpClient(handlerMock.Object)
        {
            BaseAddress = new Uri("https://api.mistral.ai/")
        };
    }

    [Fact]
    public async Task GetCompletionAsync_ReturnsExpectedText()
    {
        var json = """
        {
          "choices": [
            {
              "message": {
                "content": "Hello from Mistral"
              }
            }
          ]
        }
        """;

        var client = CreateHttpClient(json);
        var adapter = new MistralChatAdapter("fake-key", httpClient: client, delayMs: 1);

        var result = await adapter.GetCompletionAsync("system", "user");
        Assert.Equal("Hello from Mistral", result);
    }


    [Fact]
    public async Task GetCompletionAsync_RetriesOnFailure()
    {
        var successJson = """
        {
            "choices": [ { "message": { "content": "Retried success" } } ]
        }
        """;

        var client = CreateHttpClientSequence(
            new HttpResponseMessage(HttpStatusCode.InternalServerError),
            new HttpResponseMessage(HttpStatusCode.OK)
            {
                Content = new StringContent(successJson)
            }
        );

        var adapter = new MistralChatAdapter("fake", httpClient: client, delayMs: 1);

        var result = await adapter.GetCompletionAsync("sys", "msg");
        Assert.Equal("Retried success", result);
    }

    [Fact]
    public async Task GetCompletionStructuredAsync_ThrowsOnInvalidJson()
    {
        var malformedJson = """
        {
            "choices": [ { "message": { "content": "{ not valid json }" } } ]
        }
        """;

        var schema = JsonDocument.Parse("""{ "type": "object" }""");

        var client = CreateHttpClientSequence(new HttpResponseMessage(HttpStatusCode.OK)
        {
            Content = new StringContent(malformedJson)
        });

        var adapter = new MistralChatAdapter("fake", httpClient: client, delayMs: 1);

        await Assert.ThrowsAsync<JsonException>(() =>
            adapter.GetCompletionStructuredAsync("sys", "msg", schema, "book"));
    }

    [Fact]
    public async Task GetCompletionAsync_ThrowsOnHttpError()
    {
        var client = CreateHttpClientSequence(new HttpResponseMessage(HttpStatusCode.BadRequest)
        {
            Content = new StringContent("Bad request")
        });

        var adapter = new MistralChatAdapter("fake", httpClient: client, delayMs: 1);

        await Assert.ThrowsAsync<HttpRequestException>(() =>
            adapter.GetCompletionAsync("sys", "msg"));
    }

}
