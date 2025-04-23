using RAGNET.Domain.Entities;
using RAGNET.Domain.Enums;

namespace RAGNET.Domain.Repositories
{
    public interface IRepository<T>
    {
        Task<T?> GetByIdAsync(Guid id, string? userId);
        Task<IEnumerable<T>> GetAllAsync(string? userId);
        Task<T> AddAsync(T entity);
        Task UpdateAsync(T entity, string? userId);
        Task DeleteAsync(T entity, string? userId);
    }

    public interface IWorkflowRepository : IRepository<Workflow>
    {
        Task<Workflow?> GetWithRelationsAsync(Guid id, string userId);
        Task<Workflow?> GetWithRelationsByApiKey(string apiKey);
        Task<IEnumerable<Workflow>> GetUserWorkflows(string id);
        Task UpdateByApiKey(Workflow workflow, string apiKey);
    }
    public interface IUserApiKeyRepository : IRepository<UserApiKey>
    {
        Task<IEnumerable<UserApiKey>> GetByUserIdAsync(string userId);
        Task<UserApiKey?> GetByUserIdAndProviderAsync(string userId, SupportedProvider provider);
        Task<bool> ExistsAsync(SupportedProvider provider, string userId);
    }
    public interface ICallbackUrlRepository : IRepository<CallbackUrl>
    {
        Task<List<string>> GetByWorkflowAsync(Guid workflowId);
    }
    public interface IChunkerRepository : IRepository<Chunker>, IConfigMeta<Chunker> { }
    public interface IQueryEnhancerRepository : IRepository<QueryEnhancer>, IConfigMeta<QueryEnhancer> { }
    public interface IFilterRepository : IRepository<Filter>, IConfigMeta<Filter> { }
    public interface IRankerRepository : IRepository<Ranker>, IConfigMeta<Ranker> { }
    public interface IEmbeddingProviderConfigRepository : IRepository<EmbeddingProviderConfig> { }
    public interface IConversationProviderConfigRepository : IRepository<ConversationProviderConfig> { }
    public interface IChunkerMetaRepository : IRepository<ChunkerMeta> { }
    public interface IQueryEnhancerMetaRepository : IRepository<QueryEnhancerMeta> { }
    public interface IFilterMetaRepository : IRepository<FilterMeta> { }
    public interface IRankerMetaRepository : IRepository<RankerMeta> { }
    public interface IDocumentRepository : IRepository<Document> { }
    public interface IPageRepository : IRepository<Page>
    {
        Task<List<Page>> GetMany(Guid[] ids);
    }
    public interface IChunkRepository : IRepository<Chunk>
    {
        Task<Chunk?> GetByVectorId(string documentId);
        Task<List<Chunk>> GetManyByVectorId(string[] vectorIds);
    }
}