using RAGNET.Domain.SeedWork;
using RAGNET.Infrastructure.VectorDatabases.Pinecone.Specs.Pod;

namespace RAGNET.Infrastructure.VectorDatabases.Pinecone.Rules
{
    public class PodSizeMustBeValidRule(
        string? podSize
    ) : IBusinessRule
    {
        public string Message => "Invalid Pod Size for Pinecone Pod.";
        public string? PodSize = podSize;
        public bool IsBroken()
        {
            if (!string.IsNullOrEmpty(PodSize))
            {
                if (Enum.TryParse(PodSize, out PodSizes _))
                    return false;
            }
            return true;
        }
    }
}