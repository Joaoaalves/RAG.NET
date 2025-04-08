using RAGNET.Domain.Entities;
using RAGNET.Domain.Repositories;
using RAGNET.Infrastructure.Data;

namespace RAGNET.Infrastructure.Repositories
{
    public class PageRepository(ApplicationDbContext context) : Repository<Page>(context), IPageRepository { }
}