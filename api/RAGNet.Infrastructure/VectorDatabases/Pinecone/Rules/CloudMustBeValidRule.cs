using Pinecone;
using RAGNET.Domain.SeedWork;

namespace RAGNET.Infrastructure.VectorDatabases.Pinecone.Rules
{
    public class CloudMustBeValidRule(
        string? cloud
    ) : IBusinessRule
    {
        public string Message => "Invalid Cloud parameter";
        public string? Cloud = cloud;
        public bool IsBroken()
        {
            if (!string.IsNullOrEmpty(Cloud))
            {
                if (Enum.TryParse(Cloud, out ServerlessSpecCloud _))
                    return false;
            }
            return true;
        }
    }
}