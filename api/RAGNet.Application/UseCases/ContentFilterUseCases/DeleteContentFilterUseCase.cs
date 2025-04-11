using RAGNET.Domain.Repositories;

namespace RAGNET.Application.UseCases.ContentFilterUseCases
{
    public interface IDeleteContentFilterUseCase
    {
        Task<bool> Execute(Guid filterId, string userId);
    }

    public class DeleteContentFilterUseCase(IFilterRepository repo) : IDeleteContentFilterUseCase
    {
        private readonly IFilterRepository _repo = repo;
        public Task<bool> Execute(Guid filterId, string userId)
        {
            try
            {
                var filter = _repo.GetByIdAsync(filterId, userId).Result ?? throw new Exception("Filter not found.");

                _repo.DeleteAsync(filter, userId).Wait();
                return Task.FromResult(true);
            }
            catch (Exception exc)
            {
                Console.WriteLine(exc.Message);
                throw new Exception("Error deleting Filter", exc);
            }
        }
    }
}