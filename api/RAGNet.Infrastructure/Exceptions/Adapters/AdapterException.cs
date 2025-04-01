namespace RAGNET.Infrastructure.Exceptions.Adapters
{
    public class AdapterException : Exception
    {
        public AdapterException(string message)
            : base(message) { }

        public AdapterException(string message, Exception innerException)
            : base(message, innerException) { }
    }
}