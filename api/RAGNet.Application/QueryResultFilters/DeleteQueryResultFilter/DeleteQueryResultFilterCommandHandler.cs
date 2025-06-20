using RAGNET.Application.Configuration.Commands;
using RAGNET.Domain.QueryResultFilters;
using RAGNET.Domain.SeedWork;

namespace RAGNET.Application.QueryResultFilters.DeleteQueryResultFilter
{
    public class DeleteQueryResultFilterCommandHandler(
        IQueryResultFilterRepository repo,
        IUnitOfWork unitOfWork
    ) : ICommandHandler<DeleteQueryResultFilterCommand, bool>
    {
        private readonly IQueryResultFilterRepository _repo = repo;
        private readonly IUnitOfWork _unitOfWork = unitOfWork;
        public async Task<bool> Handle(DeleteQueryResultFilterCommand request, CancellationToken cancellationToken)
        {
            try
            {
                var filter = _repo.GetByIdAsync(
                    request.FilterId,
                    request.UserId
                ).Result ?? throw new Exception("QueryResultFilter not found.");

                await _repo.DeleteAsync(filter, request.UserId);
                await _unitOfWork.CommitAsync(cancellationToken);
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