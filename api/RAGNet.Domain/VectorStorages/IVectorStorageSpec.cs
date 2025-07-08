using RAGNET.Domain.SharedKernel.VectorStorages;

namespace RAGNET.Domain.VectorStorages
{
    public interface IVectorStorageSpec
    {
        VectorStorageProvider Provider { get; }
        Dictionary<string, string> ToMeta();
    }
}