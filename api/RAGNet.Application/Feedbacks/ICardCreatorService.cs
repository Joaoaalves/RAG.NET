namespace RAGNET.Application.Feedbacks
{
    public interface ICardCreatorService
    {
        Task CreateCardAsync(string title, string description);
    }
}