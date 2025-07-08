
using RAGNET.Domain.SharedKernel.Metas;
using RAGNET.Domain.SharedKernel.VectorStorages;
using RAGNET.Domain.SharedKernel.VectorStorages.Hosts;
using RAGNET.Domain.Users.ApiKeys;
using RAGNET.Domain.VectorStorages;

namespace RAGNET.Infrastructure.VectorDatabases.Qdrant.Specs
{
    public sealed class QdrantSpec : IVectorStorageSpec
    {
        public VectorStorageProvider Provider => VectorStorageProvider.QDRANT;
        public Host Host { get; init; } = null!;

        public Dictionary<string, string> ToMeta() => new()
        {
            ["host"] = Host.ToString(),
        };

        public static QdrantSpec FromMeta(IEnumerable<Meta> metas)
        {
            try
            {
                var hostString = metas.FirstOrDefault(m => m.Key == "host")?.Value ?? throw new ArgumentNullException("Host");

                return new()
                {
                    Host = new Host(hostString)
                };
            }
            catch (ArgumentNullException exc)
            {
                throw new ArgumentNullException($"Qdrant spec - {exc.Message} not found.");
            }
        }
    }
}