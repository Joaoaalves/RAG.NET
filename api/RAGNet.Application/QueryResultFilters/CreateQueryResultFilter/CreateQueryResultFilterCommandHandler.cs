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
                var filter = await _filterRepository.AddAsync(request.Filter);
                await _unitOfWork.CommitAsync(cancellationToken);
                return filter.ToDTO();
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                await _unitOfWork.RevertAsync();
                throw new Exception("Error creating query enhancer", ex);
            }
        }
    }
}