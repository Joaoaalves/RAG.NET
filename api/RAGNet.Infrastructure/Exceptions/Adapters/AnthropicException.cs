namespace RAGNET.Infrastructure.Exceptions.Adapters
{
    public class AnthropicChatException : AdapterException
    {
        public AnthropicChatException(string message) : base(message) { }

        public AnthropicChatException(string message, Exception innerException)
            : base(message, innerException) { }
    }
}