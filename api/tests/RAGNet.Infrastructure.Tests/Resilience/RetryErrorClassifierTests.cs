using RAGNET.Infrastructure.SeedWork.Resilience;

namespace tests.RAGNet.Infrastructure.Tests.Resilience
{
    public class RetryErrorClassifierTests
    {
        [Theory]
        [InlineData("429 Too Many Requests", true)]
        [InlineData("503 Service Unavailable", true)]
        [InlineData("500 Internal Server Error", true)]
        [InlineData("Connection timeout", true)]
        [InlineData("Connection reset", false)]
        [InlineData("Not Found", false)]
        public void IsTransientError_ShouldIdentifyTransientErrors(string message, bool expected)
        {
            var exception = new HttpRequestException(message);

            bool isTransient = RetryErrorClassifier.IsTransientError(exception);

            Assert.Equal(expected, isTransient);
        }

        [Fact]
        public void IsTransientError_TaskCanceled_ShouldBeTransient()
        {
            Exception ex = new TaskCanceledException();
            Assert.True(RetryErrorClassifier.IsTransientError(ex));
        }

        [Fact]
        public void IsTransientError_Other_ShouldNotBeTransient()
        {
            Exception ex = new InvalidOperationException("nope");
            Assert.False(RetryErrorClassifier.IsTransientError(ex));
        }
    }
}