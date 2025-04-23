namespace RAGNET.Infrastructure.Exceptions.Adapters
{
    public class GeminiChatException : AdapterException
    {
        public GeminiChatException(string message) : base(message) { }

        public GeminiChatException(string message, Exception innerException)
            : base(message, innerException) { }
    }

    public class GeminiEmbeddingException : AdapterException
    {
        public GeminiEmbeddingException(string message) : base(message) { }

        public GeminiEmbeddingException(string message, Exception innerException)
            : base(message, innerException) { }
    }
}