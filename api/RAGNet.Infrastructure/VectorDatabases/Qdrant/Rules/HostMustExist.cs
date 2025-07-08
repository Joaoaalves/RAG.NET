using RAGNET.Domain.SeedWork;
using RAGNET.Domain.SharedKernel.Metas;

namespace RAGNET.Infrastructure.VectorDatabases.Qdrant.Rules
{
    public class HostMustExistRule(IEnumerable<Meta> metas, string? hostKey = null) : IBusinessRule
    {
        public string Message => "The host does not exist.";
        public string HostKey { get; } = hostKey ?? "host";
        public IEnumerable<Meta> Metas = metas;

        public bool IsBroken()
        {
            var host = Metas.FirstOrDefault(meta => meta.Key == HostKey)?.Value;

            return host == null;
        }
    }
}