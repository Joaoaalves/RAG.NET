using RAGNET.Domain.Entities;
using RAGNET.Domain.Repositories;
using RAGNET.Infrastructure.Data;

namespace RAGNET.Infrastructure.Repositories
{
    public class ConversationProviderConfigRepository(ApplicationDbContext context) : Repository<ConversationProviderConfig>(context), IConversationProviderConfigRepository
    {
    }
}