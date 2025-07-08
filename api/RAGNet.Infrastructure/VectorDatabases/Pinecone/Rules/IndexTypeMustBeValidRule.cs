using RAGNET.Domain.SeedWork;

namespace RAGNET.Infrastructure.VectorDatabases.Pinecone.Rules
{
    public class IndexTypeMustBeValidRule(
        string? indexType
    ) : IBusinessRule
    {
        public string Message => "Index Type must be valid.";
        public string? IndexType = indexType;
        public bool IsBroken()
        {
            if (!string.IsNullOrEmpty(IndexType))
            {
                if (Enum.TryParse(IndexType, out PineconeIndexType indexType))
                {
                    return false;
                }
            }


            return true;
        }
    }
}