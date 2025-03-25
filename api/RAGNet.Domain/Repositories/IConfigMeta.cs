namespace RAGNET.Domain.Repositories
{
    public interface IConfigMeta<T>
    {
        Task<IEnumerable<T>> GetWithMetaAsync(Guid ID);
    }
}