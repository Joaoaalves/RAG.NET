using RAGNET.Application.Configuration.Commands;
using RAGNET.Domain.QueryResultFilters;
using RAGNET.Domain.SeedWork;

using RAGNET.Application.QueryResultFilters.DTOs;
using RAGNET.Application.QueryResultFilters.Mappers;

namespace RAGNET.Application.QueryResultFilters.Commands.UpdateQueryResultFilter
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
                var workflow = request.Workflow;
                var filterId = workflow.QueryResultFilter?.Id ?? throw new Exception("Filter not enabled!");

                var filter = await _repo.GetByIdAsync(
                    filterId,
                    request.User.Id
                ) ?? throw new Exception("QueryResultFilter not found.");

                filter.UpdateMaxItems(request.Data.MaxItems);

                if (request.Data.IsEnabled != null)
                    filter.SetEnableState(request.Data.IsEnabled.Value);

                await _repo.UpdateAsync(filter, request.User.Id);
                await _unitOfWork.CommitAsync(cancellationToken);

                return filter.ToDTO();
            }
            catch (Exception)
            {
                await _unitOfWork.RevertAsync();
                throw;
            }
        }
    }
}