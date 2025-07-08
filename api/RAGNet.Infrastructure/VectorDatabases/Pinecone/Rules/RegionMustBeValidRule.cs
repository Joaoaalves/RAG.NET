using Pinecone;
using RAGNET.Domain.SeedWork;

namespace RAGNET.Infrastructure.VectorDatabases.Pinecone.Rules
{
    public class RegionMustBeValidRule(
        string? region,
        ServerlessSpecCloud cloud
    ) : IBusinessRule
    {
        public string Message => "Invalid Region for Pinecone.";
        public string? Region = region;
        public ServerlessSpecCloud Cloud = cloud;
        public bool IsBroken()
        {
            try
            {
                if (!string.IsNullOrEmpty(Region))
                {
                    var region = Region.ToServerlessRegion(Cloud);
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