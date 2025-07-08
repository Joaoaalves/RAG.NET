using RAGNET.Domain.SharedKernel.Metas;
using RAGNET.Domain.SharedKernel.VectorStorages;
using RAGNET.Domain.VectorStorages;

namespace RAGNET.Infrastructure.VectorDatabases.Pinecone.Specs.Pod
{
    public sealed class PineconePodInfraSpec : IVectorStorageSpec
    {
        public VectorStorageProvider Provider => VectorStorageProvider.PINECONE;
        public static PineconeIndexType IndexType { get; } = PineconeIndexType.Pod;
        public string Environment { get; init; } = string.Empty;
        public PodTypes PodType { get; init; }
        public PodSizes PodSize { get; set; }
        public uint Pods { get; init; } = 1;

        public Dictionary<string, string> ToMeta() => new()
        {
            ["indexType"] = IndexType.ToString(),
            ["podSize"] = PodSize.ToString().ToLowerInvariant(),
            ["podType"] = PodType.ToString().ToLowerInvariant(),
            ["pods"] = Pods.ToString(),
            ["environment"] = Environment
        };

        public static PineconePodInfraSpec FromMeta(IEnumerable<Meta> metas)
        {
            try
            {
                var podTypeString = metas.FirstOrDefault(m => m.Key == "podType")?.Value ?? throw new ArgumentNullException("Pod Type");
                PodTypes podType = PodTypes.P1;
                _ = Enum.TryParse(podTypeString, out podType);

                var podSizeString = metas.FirstOrDefault(m => m.Key == "podSize")?.Value ?? throw new ArgumentNullException("Pod Size");
                PodSizes podSize = PodSizes.X1;
                _ = Enum.TryParse(podSizeString, out podSize);

                var environment = metas.FirstOrDefault(m => m.Key == "environment")?.Value ?? throw new ArgumentNullException("Environment.");

                var podsString = metas.FirstOrDefault(m => m.Key == "pods")?.Value ?? throw new ArgumentNullException("Pods ammount.");
                uint pods = Convert.ToUInt32(podsString);
                return new()
                {
                    PodType = podType,
                    PodSize = podSize,
                    Pods = pods,
                    Environment = environment
                };
            }
            catch (ArgumentNullException exc)
            {
                throw new ArgumentNullException($"Pinecone Pod Spec - {exc.Message} not found.");
            }
        }
    }
}