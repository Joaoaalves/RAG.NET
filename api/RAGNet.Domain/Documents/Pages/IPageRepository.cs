namespace RAGNET.Domain.Documents.Pages
{
    public interface IPageRepository
    {
        Task<Page> AddAsync(Page page);
        Task<List<Page>> GetManyAsync(PageId[] pageIds);
        Task<List<Page>> GetManyByDocumentId(DocumentId documentId);
    }
}