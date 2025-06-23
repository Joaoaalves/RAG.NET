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
                var workflow = request.Workflow;
                var filterId = workflow.QueryResultFilter?.Id ?? throw new Exception("Query Result Filter not found!");

                var filter = _repo.GetByIdAsync(
                    filterId,
                    request.User.Id
                ).Result ?? throw new Exception("QueryResultFilter not found.");

                await _repo.DeleteAsync(filter, request.User.Id);
                await _unitOfWork.CommitAsync(cancellationToken);
                return true;
            }
            catch (Exception)
            {
                await _unitOfWork.RevertAsync();
                throw;
            }
        }
    }
}