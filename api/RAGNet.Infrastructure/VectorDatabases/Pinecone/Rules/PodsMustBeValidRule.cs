using RAGNET.Domain.SeedWork;

namespace RAGNET.Infrastructure.VectorDatabases.Pinecone.Rules
{
    public class PodsMustBeValidRule(
        string? pods
    ) : IBusinessRule
    {
        public string Message => "Invalid Pod Ammount for Pinecone Pod.";
        public string? Pods = pods;
        public bool IsBroken()
        {
            try
            {
                if (!string.IsNullOrEmpty(Pods))
                {
                    var podAmount = Convert.ToUInt32(Pods);
                    return false;
                }
                return true;
            }
            catch (Exception)
            {
                return true;
            }

        }
    }
}