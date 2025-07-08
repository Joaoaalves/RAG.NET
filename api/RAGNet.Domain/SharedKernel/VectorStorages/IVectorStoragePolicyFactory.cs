namespace RAGNET.Domain.SharedKernel.VectorStorages
{
    public interface IVectorStoragePolicyFactory
    {
        IVectorStoragePolicy CreatePolicy(VectorStorageProvider provider);
    }
}