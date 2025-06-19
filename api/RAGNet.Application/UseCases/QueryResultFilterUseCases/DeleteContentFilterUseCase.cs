using RAGNET.Domain.QueryResultFilters;
using RAGNET.Domain.SeedWork;

namespace RAGNET.Application.UseCases.QueryResultFilterUseCases
{
    public interface IDeleteQueryResultFilterUseCase
    {
        Task<bool> Execute(QueryResultFilterId filterId, string userId);
    }

    public class DeleteQueryResultFilterUseCase(IQueryResultFilterRepository repo, IUnitOfWork unitOfWork) : IDeleteQueryResultFilterUseCase
    {
        private readonly IQueryResultFilterRepository _repo = repo;
        private readonly IUnitOfWork _unitOfWork = unitOfWork;
        public async Task<bool> Execute(QueryResultFilterId filterId, string userId)
        {
            try
            {
                var filter = _repo.GetByIdAsync(
                    filterId,
                    userId
                ).Result ?? throw new Exception("QueryResultFilter not found.");

                _repo.DeleteAsync(filter, userId).Wait();
                await _unitOfWork.CommitAsync();
                return true;
            }
            catch (Exception exc)
            {
                await _unitOfWork.RevertAsync();
                Console.WriteLine(exc.Message);
                throw new Exception("Error deleting QueryResultFilter", exc);
            }
        }
    }
}