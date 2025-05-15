using RAGNET.Domain.Entities;

namespace RAGNET.Domain.Services
{
    public interface ICardCreatorService
    {
        Task CreateCardAsync(Card card);
    }
}