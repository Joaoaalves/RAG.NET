namespace RAGNET.Application.Feedback
{
    public interface ICardCreatorService
    {
        Task CreateCardAsync(string title, string description);
    }
}