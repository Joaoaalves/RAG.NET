using RAGNET.Domain.SeedWork;

namespace RAGNET.Infrastructure.VectorDatabases.Pinecone.Rules
{
    public class EnvironmentMustBeValidRule(
        string? environment
    ) : IBusinessRule
    {
        public string Message => "Invalid Environment for Pinecone.";
        public string? Environment = environment;
        public bool IsBroken()
        {
            if (!string.IsNullOrEmpty(Environment))
                return false;

            return true;
        }
    }
}