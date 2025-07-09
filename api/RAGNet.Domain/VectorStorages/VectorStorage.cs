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
        public ApiKey ApiKey { get; private set; } = null!;
        public VectorStorageProvider Provider { get; private set; }
        public bool IsActive { get; private set; } = true;
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
                if (!string.IsNullOrEmpty(value))
                    _metas.Add(new Meta(key, value));
        }

        public void UpdateMetas(Dictionary<string, string> metas)
        {
            foreach (var meta in metas)
            {
                if (!string.IsNullOrEmpty(meta.Value))
                    _metas.FirstOrDefault(m => m.Key == meta.Key)?.UpdateValue(meta.Value);
            }
        }

        public void SetIsActive(bool isActive)
        {
            IsActive = isActive;
        }

        public void UpdateApiKey(ApiKey newApiKey)
        {
            ApiKey = newApiKey;
        }
    }

}