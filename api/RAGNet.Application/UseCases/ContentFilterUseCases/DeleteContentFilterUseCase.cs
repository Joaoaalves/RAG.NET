using RAGNET.Domain.Filters;
using RAGNET.Domain.SeedWork;

namespace RAGNET.Application.UseCases.ContentFilterUseCases
{
    public interface IDeleteContentFilterUseCase
    {
        Task<bool> Execute(Guid filterId, string userId);
    }

    public class DeleteContentFilterUseCase(IFilterRepository repo, IUnitOfWork unitOfWork) : IDeleteContentFilterUseCase
    {
        private readonly IFilterRepository _repo = repo;
        private readonly IUnitOfWork _unitOfWork = unitOfWork;
        public async Task<bool> Execute(Guid filterId, string userId)
        {
            try
            {
                var filter = _repo.GetByIdAsync(filterId, userId).Result ?? throw new Exception("Filter not found.");

                _repo.DeleteAsync(filter, userId).Wait();
                await _unitOfWork.CommitAsync();
                return true;
            }
            catch (Exception exc)
            {
                await _unitOfWork.RevertAsync();
                Console.WriteLine(exc.Message);
                throw new Exception("Error deleting Filter", exc);
            }
        }
    }
}