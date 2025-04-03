namespace RAGNET.Infrastructure.Exceptions.Adapters
{
    public class QueryEnhancerChatException : AdapterException
    {
        public QueryEnhancerChatException(string message) : base(message) { }

        public QueryEnhancerChatException(string message, Exception innerException)
            : base(message, innerException) { }
    }
}