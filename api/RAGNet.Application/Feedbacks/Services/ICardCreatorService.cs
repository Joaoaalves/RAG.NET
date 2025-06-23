namespace RAGNET.Application.Feedbacks.Services
{
    public interface ICardCreatorService
    {
        Task CreateCardAsync(string title, string description);
    }
}