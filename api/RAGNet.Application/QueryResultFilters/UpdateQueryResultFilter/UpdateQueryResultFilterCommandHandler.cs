using RAGNET.Application.Configuration.Commands;
using RAGNET.Domain.QueryResultFilters;
using RAGNET.Domain.SeedWork;

namespace RAGNET.Application.QueryResultFilters.UpdateQueryResultFilter
{
    public class UpdateQueryResultFilterCommandHandler(
        IQueryResultFilterRepository _repo,
        IUnitOfWork unitOfWork
    ) : ICommandHandler<UpdateQueryResultFilterCommand, QueryResultFilterDTO>
    {
        private readonly IQueryResultFilterRepository _repo = _repo;
        private readonly IUnitOfWork _unitOfWork = unitOfWork;
        public async Task<QueryResultFilterDTO> Handle(UpdateQueryResultFilterCommand request, CancellationToken cancellationToken)
        {
            try
            {
                var filter = await _repo.GetByIdAsync(
                    request.FilterId,
                    request.UserId
                ) ?? throw new Exception("QueryResultFilter not found.");

                filter.UpdateMaxItems(request.Data.MaxItems);

                if (request.Data.IsEnabled != null)
                    filter.SetEnableState(request.Data.IsEnabled.Value);

                await _repo.UpdateAsync(filter, request.UserId);
                await _unitOfWork.CommitAsync(cancellationToken);

                return filter.ToDTO();
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                await _unitOfWork.RevertAsync();
                throw new Exception("Error updating content filter.", ex);
            }
        }
    }
}