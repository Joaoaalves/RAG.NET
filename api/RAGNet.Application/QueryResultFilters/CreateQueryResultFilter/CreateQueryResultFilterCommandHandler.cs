using RAGNET.Application.Configuration.Commands;
using RAGNET.Domain.QueryResultFilters;
using RAGNET.Domain.SeedWork;

namespace RAGNET.Application.QueryResultFilters.CreateQueryResultFilter
{
    public class CreateQueryResultFilterCommandHandler(
        IQueryResultFilterRepository filterRepository,
        IUnitOfWork unitOfWork
    ) : ICommandHandler<CreateQueryResultFilterCommand, QueryResultFilterDTO>
    {
        private readonly IUnitOfWork _unitOfWork = unitOfWork;

        private readonly IQueryResultFilterRepository _filterRepository = filterRepository;

        public async Task<QueryResultFilterDTO> Handle(CreateQueryResultFilterCommand request, CancellationToken cancellationToken)
        {
            try
            {
                var workflow = request.Workflow;

                if (workflow.QueryResultFilter != null && workflow.QueryResultFilter.IsEnabled)
                    throw new Exception("Relevant Segment Extraction already enabled!");

                var filterData = request.Data.ToFilter(workflow.Id, request.User.Id);

                var filter = await _filterRepository.AddAsync(filterData);

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