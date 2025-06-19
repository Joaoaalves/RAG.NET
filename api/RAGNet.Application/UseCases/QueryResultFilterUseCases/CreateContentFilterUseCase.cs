using RAGNET.Domain.QueryResultFilters;
using RAGNET.Domain.SeedWork;
using RAGNET.Domain.Workflows;

namespace RAGNET.Application.UseCases.QueryResultFilterUseCases
{
    public interface ICreateQueryResultFilterUseCase
    {
        Task<QueryResultFilter> Execute(QueryResultFilter filter, WorkflowId workflowId, string userId);
    }

    public class CreateQueryResultFilterUseCase(IQueryResultFilterRepository filterRepository, IUnitOfWork unitOfWork) : ICreateQueryResultFilterUseCase
    {
        private readonly IQueryResultFilterRepository _filterRepository = filterRepository;
        private readonly IUnitOfWork _unitOfWork = unitOfWork;
        public async Task<QueryResultFilter> Execute(QueryResultFilter filter, WorkflowId workflowId, string userId)
        {
            try
            {
                await _filterRepository.AddAsync(filter);
                await _unitOfWork.CommitAsync();
                return filter;
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