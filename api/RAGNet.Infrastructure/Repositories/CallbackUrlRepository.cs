using RAGNET.Domain.Entities;
using RAGNET.Domain.Repositories;
using RAGNET.Infrastructure.Data;

namespace RAGNET.Infrastructure.Repositories
{
    public class CallbackUrlRepository(ApplicationDbContext context) : Repository<CallbackUrl>(context), ICallbackUrlRepository
    { }
}