using Moq;
using RAGNET.Domain.Documents.Pages.Chunks;
using RAGNET.Infrastructure.Embedders.OpenAI;

namespace tests.RAGNet.Infrastructure.Tests.Embedders;

public class OpenAIEmbeddingAdapterTests
{
    private readonly Mock<IOpenAIEmbeddingWrapper> _mockWrapper;
    private readonly int _delayMs = 1;

    public OpenAIEmbeddingAdapterTests()
    {
        _mockWrapper = new Mock<IOpenAIEmbeddingWrapper>(MockBehavior.Strict);
    }

    private OpenAIEmbeddingAdapter CreateAdapter() =>
        new(_mockWrapper.Object, _delayMs);

    [Fact]
    public async Task GetEmbeddingAsync_ReturnsEmbedding_OnSuccess()
    {
        // Arrange
        var input = "Hello";
        var expected = new SemanticVector([0.1f, 0.2f]);

        _mockWrapper
            .Setup(x => x.GenerateEmbeddingAsync(input, default))
            .ReturnsAsync(expected);

        var adapter = CreateAdapter();

        // Act
        var result = await adapter.GetEmbeddingAsync(input);

        // Assert
        Assert.Equal(expected, result);
    }

    [Fact]
    public async Task GetMultipleEmbeddingAsync_ReturnsAllEmbeddings()
    {
        // Arrange
        var inputs = new[] { "A", "B" };
        var expected = new[]
        {
            new SemanticVector([1f]),
            new SemanticVector([2f])
        };

        _mockWrapper
            .Setup(x => x.GenerateEmbeddingAsync("A", default))
            .ReturnsAsync(expected[0]);

        _mockWrapper
            .Setup(x => x.GenerateEmbeddingAsync("B", default))
            .ReturnsAsync(expected[1]);

        var adapter = CreateAdapter();

        // Act
        var result = await adapter.GetMultipleEmbeddingAsync(inputs.ToList());

        // Assert
        Assert.Equal(2, result.Count);
        Assert.Equal(expected[0], result[0]);
        Assert.Equal(expected[1], result[1]);
    }

    [Fact]
    public async Task GetEmbeddingAsync_RetriesOnFailure_ThenSucceeds()
    {
        // Arrange
        var input = "Retry text";
        var expected = new SemanticVector([9f, 8f]);
        int attempts = 0;

        _mockWrapper
            .Setup(x => x.GenerateEmbeddingAsync(input, default))
            .ReturnsAsync(() =>
            {
                attempts++;
                if (attempts < 3)
                    throw new HttpRequestException("timeout");
                return expected;
            });

        var adapter = CreateAdapter();

        // Act
        var result = await adapter.GetEmbeddingAsync(input);

        // Assert
        Assert.Equal(expected, result);
        Assert.Equal(3, attempts);
    }

    [Fact]
    public async Task GetEmbeddingAsync_FailsAfterMaxRetries()
    {
        // Arrange
        var input = "Always fails";

        _mockWrapper
            .Setup(x => x.GenerateEmbeddingAsync(input, default))
            .ThrowsAsync(new Exception("Permanent failure"));

        var adapter = CreateAdapter();

        // Act & Assert
        await Assert.ThrowsAsync<Exception>(() => adapter.GetEmbeddingAsync(input));
        _mockWrapper.Verify(x => x.GenerateEmbeddingAsync(input, default), Times.AtLeastOnce);
    }
}
