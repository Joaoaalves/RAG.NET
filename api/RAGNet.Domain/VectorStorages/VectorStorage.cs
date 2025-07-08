using RAGNET.Domain.SeedWork;
using RAGNET.Domain.SharedKernel.Metas;
using RAGNET.Domain.SharedKernel.VectorStorages;
using RAGNET.Domain.Users.ApiKeys;

namespace RAGNET.Domain.VectorStorages
{
    public class VectorStorage : Entity, IUserOwned
    {
        public VectorStorageId Id { get; private init; } = null!;
        public string UserId { get; set; } = string.Empty;
        public ApiKey ApiKey { get; set; } = null!;
        public VectorStorageProvider Provider { get; private set; }

        private readonly List<Meta> _metas = [];
        public IReadOnlyCollection<Meta> Metas => _metas.AsReadOnly();

        private VectorStorage() { }

        private VectorStorage(
            VectorStorageId id,
            string userId,
            ApiKey apiKey,
            VectorStorageProvider provider)
        {
            Id = id;
            UserId = userId;
            ApiKey = apiKey;
            Provider = provider;
        }

        public static VectorStorage Create(
            VectorStorageProvider provider,
            string userId,
            ApiKey apiKey,
            VectorStorageId? id = null)
        {
            return new VectorStorage(id ?? new(), userId, apiKey, provider);
        }

        public void SetSpecMetas(Dictionary<string, string> specMeta)
        {
            _metas.Clear();
            foreach (var (key, value) in specMeta)
                _metas.Add(new Meta(key, value));
        }
    }

}