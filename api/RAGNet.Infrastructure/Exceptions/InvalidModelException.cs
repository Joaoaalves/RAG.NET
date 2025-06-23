namespace RAGNET.Infrastructure.Exceptions
{
    public class InvalidModelException(string message) : InfrastructureException(message) { }
    public class InvalidConversationModelException(string message) : InvalidModelException(message)
    {
    }

    public class InvalidEmbeddingModelException(string message) : InvalidModelException(message)
    {
    }
}