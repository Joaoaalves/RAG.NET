using RAGNET.Domain.SharedKernel.Metas;

namespace RAGNET.Domain.SharedKernel.VectorStorages
{
    public interface IVectorStoragePolicy
    {
        VectorStorageProvider ProviderType { get; }
        string Name { get; }
        string Prefix { get; }
        string Pattern { get; }
        string Url { get; }
        string Schema { get; }
        void Validate(string apiKey, IEnumerable<Meta> metas);
    }
}