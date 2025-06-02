using RAGNET.Domain.Documents;
using RAGNET.Domain.Entities;
using RAGNET.Domain.Repositories;
using RAGNET.Infrastructure.Data;

namespace RAGNET.Infrastructure.Repositories
{
    public class DocumentRepository(ApplicationDbContext context) : Repository<Document>(context), IDocumentRepository
    {
    }
}