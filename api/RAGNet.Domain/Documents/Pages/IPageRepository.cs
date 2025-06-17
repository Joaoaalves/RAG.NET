namespace RAGNET.Domain.Documents.Pages
{
    public interface IPageRepository
    {
        Task<Page> AddAsync(Page page);
        Task<List<Page>> GetManyAsync(Guid[] pageIds);
        Task<List<Page>> GetManyByDocumentId(Guid documentId);
    }
}