using RAGNET.Domain.Entities;

namespace RAGNET.Domain.Repositories
{
    public interface IRepository<T>
    {
        Task<T?> GetByIdAsync(Guid id);
        Task<IEnumerable<T>> GetAllAsync();
        Task AddAsync(T entity);
        Task UpdateAsync(T entity);
        Task DeleteAsync(T entity);
    }

    public interface IWorkflowRepository : IRepository<Workflow>
    {
        Task<Workflow?> GetWithRelationsAsync(Guid id);
        Task<IEnumerable<Workflow>> GetUserWorkflows(string id);
    }

    public interface IChunkerRepository : IRepository<Chunker>, IConfigMeta<Chunker> { }
    public interface IQueryEnhancerRepository : IRepository<QueryEnhancer>, IConfigMeta<QueryEnhancer> { }
    public interface IFilterRepository : IRepository<Filter>, IConfigMeta<Filter> { }
    public interface IRankerRepository : IRepository<Ranker>, IConfigMeta<Ranker> { }
    public interface IChunkRepository : IRepository<Chunk> { }
    public interface IChunkerMetaRepository : IRepository<ChunkerMeta> { }
    public interface IQueryEnhancerMetaRepository : IRepository<QueryEnhancerMeta> { }
    public interface IFilterMetaRepository : IRepository<FilterMeta> { }
    public interface IRankerMetaRepository : IRepository<RankerMeta> { }
}