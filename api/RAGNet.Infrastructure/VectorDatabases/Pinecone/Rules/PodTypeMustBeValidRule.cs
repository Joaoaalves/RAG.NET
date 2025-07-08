using RAGNET.Domain.SeedWork;
using RAGNET.Infrastructure.VectorDatabases.Pinecone.Specs.Pod;

namespace RAGNET.Infrastructure.VectorDatabases.Pinecone.Rules
{
    public class PodTypeMustBeValidRule(
        string? podType
    ) : IBusinessRule
    {
        public string Message => "Invalid Pod Type for Pinecone Pod.";
        public string? PodType = podType;
        public bool IsBroken()
        {
            if (!string.IsNullOrEmpty(PodType))
            {
                if (Enum.TryParse(PodType, out PodTypes _))
                    return false;
            }
            return true;
        }
    }
}